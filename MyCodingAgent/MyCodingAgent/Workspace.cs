using MyCodingAgent.Services;

namespace MyCodingAgent;

using System.Text.Json;


public class Workspace(string currentDirectoryName)
{
    public readonly DirectoryInfo CurrentDirectory = new(currentDirectoryName);
    public readonly Dictionary<string, WorkspaceFile> FileRepository = new();
    public readonly Dictionary<int, string> Memory = new();
    public readonly Dictionary<int, string> Tasks = new();
    public readonly Queue<string> History = new();
    public WorkspaceFile? CurrentFile;
    public string CompileResult = string.Empty;

    public Task InitializeAsync()
    {
        FileRepository.Clear();

        if (!CurrentDirectory.Exists)
            CurrentDirectory.Create();

        ReadDirectory(CurrentDirectory);

        return CompileAsync();
    }
    public async Task CompileAsync()
    {
        var compileErrors = await Compiler.Compile(CurrentDirectory);

        CompileResult = compileErrors
            .Replace(CurrentDirectory.FullName + "\\", "");
    }
    public string GeneratePrompt(string userPrompt)
    {
        var request = new AgentRequest(
            CurrentFile == null ? null : new AgentFile(CurrentFile.RelativePath, CurrentFile.FileContent),
            [.. FileRepository.Values.Select(a => new AgentFileSummery(a.RelativePath, a.LineCount))],
            CompileResult,
            [.. Tasks.Select(a => new AgentTask(a.Key, a.Value))],
            [.. Memory.Select(a => new AgentMemory(a.Key, a.Value))],
            [.. History.Select(a => a)]);

        var requestJson = JsonSerializer.Serialize(request);

        return $@"You are an autonomous software engineering agent operating inside a .NET development workspace.

You interact with this system through a JSON command protocol. The system behaves like a minimal Visual Studio console environment.
Your goal is to inspect the workspace, modify files, and resolve compile errors in order to fulfill the user's request.

IMPORTANT RULES

1. Your response MUST be valid JSON.
2. The JSON MUST contain an array called ""actions"".
3. Do NOT include explanations outside the JSON.
4. Only use the actions listed below.
5. Prefer minimal edits when possible (use partial_overwrite_file).
6. Work iteratively: inspect → edit → compile → fix errors.
7. If you need file content, open the file first.

COMMAND PROTOCOL

Each action has the following structure:

{{
  ""type"": ""action_name"",
  ""parameters"": ...
}}

AVAILABLE ACTIONS

Open a file from the workspace:

{{
  ""type"": ""open_workspace_file"",
  ""path"": ""src/Program.cs""
}}

Create or overwrite a file:

{{
  ""type"": ""create_or_update_file"",
  ""path"": ""src/MyClass.cs"",
  ""content"": ""full file content here""
}}

Delete a file:

{{
  ""type"": ""delete_file"",
  ""path"": ""src/OldFile.cs""
}}

Move or rename a file:

{{
  ""type"": ""move_file"",
  ""path"": ""src/OldName.cs"",
  ""newPath"": ""src/NewName.cs""
}}

Create a directory:

{{
  ""type"": ""create_directory"",
  ""path"": ""src/services""
}}

Delete a directory:

{{
  ""type"": ""delete_directory"",
  ""path"": ""src/old""
}}

Overwrite specific lines in a file:

{{
  ""type"": ""partial_overwrite_file"",
  ""path"": ""src/Program.cs"",
  ""startLine"": 10,
  ""endLine"": 20,
  ""content"": ""replacement lines""
}}

Store persistent memory:

{{
  ""type"": ""create_or_update_memory"",
  ""id"": 1,
  ""content"": ""important information about the project""
}}

Delete stored memory:

{{
  ""type"": ""delete_memory"",
  ""id"": 1
}}

Create or update a task:

{{
  ""type"": ""create_or_update_task"",
  ""id"": 1,
  ""content"": ""Implement user authentication""
}}

Delete a task:

{{
  ""type"": ""delete_task"",
  ""id"": 1
}}

RESPONSE FORMAT

You must ALWAYS respond using this JSON structure:

{{
  ""actions"": []
}}

USER REQUEST

{userPrompt}

WORKSPACE STATE

The following JSON describes the current workspace.

{requestJson}
";
    }

    public async Task<bool> ProcessResponse(string responseText)
    {
        if (!TryParseActions(responseText, out var response))
            return false;

        var found = false;
        foreach (var action in response.Actions)
        {
            found = true;
            switch (action.Type)
            {
                case "open_workspace_file":
                    await OpenWorkspaceFile(action.Path!);
                    break;

                case "create_or_update_file":
                    await CreateOrUpdateFile(action.Path!, action.Content!);
                    break;

                case "delete_file":
                    await DeleteFile(action.Path!);
                    break;

                case "move_file":
                    await MoveFile(action.Path!, action.NewPath!);
                    break;

                case "create_directory":
                    await CreateDirectory(action.Path!);
                    break;

                case "delete_directory":
                    await RemoveDirectory(action.Path!);
                    break;

                case "partial_overwrite_file":
                    await PartialOverwriteFile(
                        action.Path!,
                        action.StartLine!.Value,
                        action.EndLine!.Value,
                        action.Content!
                    );
                    break;

                case "create_or_update_memory":
                    AddOrUpdateMemory(action.Id!.Value, action.Content!);
                    break;

                case "delete_memory":
                    DeleteMemory(action.Id!.Value);
                    break;

                case "create_or_update_task":
                    AddOrUpdateTask(action.Id!.Value, action.Content!);
                    break;

                case "delete_task":
                    DeleteTask(action.Id!.Value);
                    break;

                default:
                    found = false;
                    break;
            }
        }
        return found;
    }

    private bool TryParseActions(string responseText, out AgentResponse response)
    {
        response = new AgentResponse();
        var json = Clean(responseText);
        try
        {
            var newResponse = JsonSerializer.Deserialize<AgentResponse>(json);
            if (newResponse == null) return false;
            response = newResponse;
            return true;
        }
        catch (Exception ex)
        {
            History.Enqueue($"Could not parse response: {ex.Message}");
            return false;
        }
    }
    string Clean(string input)
    {
        input = input.Trim();

        if (input.StartsWith("```"))
        {
            var firstNewline = input.IndexOf('\n');
            var lastFence = input.LastIndexOf("```");

            if (firstNewline >= 0 && lastFence > firstNewline)
            {
                input = input.Substring(firstNewline + 1, lastFence - firstNewline - 1);
            }
        }

        return input.Trim();
    }

    // MCP INTERFACE
    private async Task OpenWorkspaceFile(string path)
    {
        if (FileRepository.TryGetValue(path, out CurrentFile))
        {
            History.Enqueue($"Opened workspace file '{path}'");
        }
        else
        {
            History.Enqueue($"Tried to open workspace file '{path}': Error, file not found");
        }
    }
    private async Task CreateDirectory(string path)
    {
        TryParseFullPath(path, out var fullPath);
        try
        {
            Directory.CreateDirectory(fullPath);
            History.Enqueue($"Created directory '{path}'");
        }
        catch (Exception ex)
        {
            History.Enqueue($"Tried to created directory '{path}': Error {ex}");
        }
    }
    private async Task RemoveDirectory(string path)
    {
        TryParseFullPath(path, out var fullPath);
        try
        {
            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true);
                History.Enqueue($"Removed directory '{path}'");
            }
            else
            {
                History.Enqueue($"Tried to remove directory '{path}': Error directory does not exist");
            }
        }
        catch (Exception ex)
        {
            History.Enqueue($"Tried to remove directory '{path}': Error {ex}");
        }
    }
    private async Task CreateOrUpdateFile(string path, string fullContent)
    {
        TryParseFullPath(path, out var fullPath);
        var fileInfo = new FileInfo(fullPath);
        if (fileInfo.Directory == null)
            throw new Exception($"Weird stuff, directory is empty? {fileInfo}");
        if (fileInfo.Directory.Exists == false)
            fileInfo.Directory.Create();

        await File.WriteAllTextAsync(fullPath, fullContent);

        FileRepository[path] =
            new WorkspaceFile(path, new FileInfo(fullPath), fullContent);

        History.Enqueue($"Updated {path}");
    }
    private async Task PartialOverwriteFile(string path, int startLineNr, int endLineNr, string newContent)
    {
        TryParseFullPath(path, out var fullPath);
        var file = FileRepository[path];
        var lines = file.FileContent.Split('\n').ToList();
        var newLines = newContent.Split('\n');

        lines.RemoveRange(startLineNr - 1, endLineNr - startLineNr + 1);
        lines.InsertRange(startLineNr - 1, newLines);

        var content = string.Join("\n", lines);

        await File.WriteAllTextAsync(fullPath, content);

        FileRepository[path] =
            new WorkspaceFile(path, new FileInfo(fullPath), content);

        History.Enqueue($"Updated {path}");
    }
    private async Task MoveFile(string path, string newPath)
    {
        TryParseFullPath(path, out var fullPath);
        var newFullPath = Path.Combine(CurrentDirectory.FullName, newPath);

        if (File.Exists(fullPath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(newFullPath)!);

            File.Move(fullPath, newFullPath, true);
            FileRepository.Remove(path);

            var content = await File.ReadAllTextAsync(newFullPath);
            FileRepository[newPath] =
                new WorkspaceFile(path, new FileInfo(fullPath), content);

            History.Enqueue($"Moved {path} -> {newPath}");
        }
    }
    private async Task DeleteFile(string path)
    {
        TryParseFullPath(path, out var fullPath);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            FileRepository.Remove(path);

            History.Enqueue($"Deleted {path}");
        }
    }
    private void AddOrUpdateMemory(int id, string content)
    {
        Memory[id] = content;
    }
    private void DeleteMemory(int id)
    {
        Memory.Remove(id);
    }
    private void AddOrUpdateTask(int id, string content)
    {
        Tasks[id] = content;
    }
    private void DeleteTask(int id)
    {
        Tasks.Remove(id);
    }
    private void TryParseFullPath(string path, out string fullPath)
    {
        fullPath = Path.GetFullPath(
            Path.Combine(CurrentDirectory.FullName, path));

        if (!fullPath.StartsWith(CurrentDirectory.FullName))
            throw new Exception($"LLM tries to hack me: {path}");
    }
    private void ReadDirectory(DirectoryInfo directoryInfo)
    {
        foreach (var dir in directoryInfo.GetDirectories())
        {
            ReadDirectory(dir);
        }

        foreach (var file in directoryInfo.GetFiles())
        {
            var relative = Path.GetRelativePath(CurrentDirectory.FullName, file.FullName);

            using var reader = new StreamReader(file.FullName);
            string fileContent = reader.ReadToEnd();

            FileRepository[relative] = new WorkspaceFile(relative, file, fileContent);
        }
    }

    public record AgentFile(
        string path,
        string content);
    public record AgentFileSummery(
        string path,
        int lineCount);
    public record AgentMemory(
        int id,
        string content);
    public record AgentTask(
        int id,
        string content);
    public record AgentRequest(
        AgentFile? current_file,
        AgentFileSummery[] files,
        string compiler_output,
        AgentTask[] tasks,
        AgentMemory[] memory,
        string[] history);
}
