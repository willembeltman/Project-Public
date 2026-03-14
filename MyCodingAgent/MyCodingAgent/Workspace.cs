using MyCodingAgent.Compile;
using System.Text.Json;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;

namespace MyCodingAgent;

public class Workspace
{
    public string RootDirectoryName { get; set; } = string.Empty;
    public string UserPrompt { get; set; } = string.Empty;
    public bool UserPromptDone { get; set; }
    public string? CurrentOpenFile { get; set; }
    public Dictionary<string, WorkspaceFile> Files { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public Dictionary<string, string> Tasks { get; set; } = new();
    public List<AgentResponseResult> AgentResponseResults { get; set; } = new();
    public string? SearchText { get; set; }
    public int PromptIndex { get; set; }

    public async static Task<Workspace?> TryLoad(string rootDirectoryName, CancellationToken ct = default)
    {
        var llmFileString = Path.Combine(rootDirectoryName, "workspace.llm");
        var workspace = (Workspace?)null;
        if (File.Exists(llmFileString))
        {
            using (var stream = File.OpenRead(llmFileString))
            {
                workspace = JsonSerializer.Deserialize<Workspace>(stream);
            }

            if (workspace != null)
            {
                var rootDirectory = new DirectoryInfo(rootDirectoryName);
                workspace.Files.Clear();
                await workspace.ReadDirectory(rootDirectory); // voor de zekerheid
                await workspace.Save();
            }
        }

        return workspace;
    }
    public async static Task<Workspace> Create(string rootDirectoryName, string userPrompt)
    {
        var workspace = new Workspace()
        {
            RootDirectoryName = rootDirectoryName,
            UserPrompt = userPrompt,
        };

        var rootDirectory = new DirectoryInfo(rootDirectoryName);
        if (!rootDirectory.Exists)
            rootDirectory.Create();

        await workspace.ReadDirectory(rootDirectory);
        await workspace.Save();
        return workspace;
    }
    public async Task Save()
    {
        var llmFileString = Path.Combine(RootDirectoryName, "workspace.llm");
        if (File.Exists(llmFileString))
            File.Delete(llmFileString);

        using var stream = File.OpenWrite(llmFileString);
        await JsonSerializer.SerializeAsync(stream, this);
    }

    public async Task<CompileResult> Compile()
    {
        var currentDirectory = new DirectoryInfo(RootDirectoryName);
        var compileResult = await Compiler.Compile(currentDirectory);
        return compileResult;
    }
    public Task<string> Search(string searchText)
    {
        SearchText = searchText;
        return Task.FromResult($"Changed search text to: '{searchText}'");
    }
    public Task<string> OpenFile(string path)
    {
        if (Files.TryGetValue(path, out _))
        {
            CurrentOpenFile = path;
            return Task.FromResult($"Opened workspace file '{path}'");
        }
        else
        {
            return Task.FromResult($"Error opening file '{path}': file not found");
        }
    }
    public Task<string> CreateDirectory(string path)
    {
        TryParseFullPath(path, out var fullPath);

        try
        {
            Directory.CreateDirectory(fullPath);
            return Task.FromResult($"Created directory '{path}'");
        }
        catch (Exception ex)
        {
            return Task.FromResult($"Error while creating directory '{path}': {ex}");
        }
    }
    public Task<string> RemoveDirectory(string path)
    {
        TryParseFullPath(path, out var fullPath);

        try
        {
            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true);
                return Task.FromResult($"Removed directory '{path}'");
            }
            else
            {
                return Task.FromResult($"Tried to remove directory '{path}': Error directory does not exist");
            }
        }
        catch (Exception ex)
        {
            return Task.FromResult($"Error while removing directory '{path}': {ex}");
        }
    }
    public Task<string> MoveFile(string path, string newPath)
    {
        if (!Files.TryGetValue(path, out var file))
            return Task.FromResult($"Error could not find '{path}'");

        TryParseFullPath(newPath, out var newFullPath);

        try
        {
            if (file.Exists())
            {
                file.Move(newPath, newFullPath);
                return $"Moved {path} -> {newPath}";
            }
            return Task.FromResult($"Error while moving: could not find '{path}'");
        }
        catch (Exception ex)
        {
            return Task.FromResult($"Error while moving '{path}': {ex.Message}");
        }
    }
    public Task<string> DeleteFile(string path)
    {
        if (!Files.TryGetValue(path, out var file))
            return Task.FromResult($"Error could not find '{path}'");

        try
        {
            if (file.Exists())
            {
                file.Delete();
                Files.Remove(path);
                return Task.FromResult($"Deleted '{path}'");
            }
            return Task.FromResult($"Error while deleting: could not find '{path}'");
        }
        catch (Exception ex)
        {
            return Task.FromResult($"Error while deleting '{path}': {ex.Message}");
        }
    }
    public async Task<string> CreateOrUpdateFile(string path, string content)
    {
        TryParseFullPath(path, out var fullPath);

        try
        {
            var file = new WorkspaceFile(path, fullPath);
            await file.UpdateContent(content);
            Files[path] = file;
            return $"Updated {path}";
        }
        catch (Exception ex)
        {
            return $"Error while updating '{path}': {ex.Message}";
        }
    }
    public async Task<string> PartialOverwriteFile(string path, int startLineNr, int endLineNr, string newContent)
    {
        if (!Files.TryGetValue(path, out var file))
            return $"Error could not find '{path}'";

        try
        {
            await file.UpdateContent(startLineNr, endLineNr, newContent);
            return $"Updated '{path}'";
        }
        catch (Exception ex)
        {
            return $"Error while updating '{path}': {ex.Message}";
        }
    }
    public async Task<string> FindAndReplace(string path, string searchText, string replaceText)
    {
        SearchText = searchText;
        if (!Files.TryGetValue(path, out var file))
            return $"Error could not find '{path}'";

        var content = await file.GetFileContent();
        var fileChanges = Regex.Matches(content, Regex.Escape(searchText)).Count;
        content = content.Replace(searchText, replaceText);

        if (fileChanges > 0)
        {
            await file.UpdateContent(content);
        }

        return $"Replaced '{searchText}' with '{replaceText}', {fileChanges} found";
    }
    public async Task<string> FindAndReplaceAll(string searchText, string replaceText)
    {
        SearchText = searchText;

        var allChanges = 0;
        foreach (var file in Files.Values)
        {
            var content = await file.GetFileContent();
            var fileChanges = Regex.Matches(content, Regex.Escape(searchText)).Count;
            content = content.Replace(searchText, replaceText);

            if (fileChanges > 0)
            {
                await file.UpdateContent(content);
            }
        }

        return $"Replaced '{searchText}' with '{replaceText}', {allChanges} found";
    }
    public Task<string> CreateOrUpdateTask(string id, string content)
    {
        Tasks[id] = content;
        return Task.FromResult($"Updated task '{id}'");
    }
    public Task<string> DeleteTask(string id)
    {
        Tasks.Remove(id);
        return Task.FromResult($"Removed task '{id}'");
    }

    private void TryParseFullPath(string path, out string fullPath)
    {
        var currentDirectory = new DirectoryInfo(RootDirectoryName);

        fullPath = Path.GetFullPath(
            Path.Combine(currentDirectory.FullName, path));

        if (!fullPath.StartsWith(currentDirectory.FullName + Path.DirectorySeparatorChar))
            throw new Exception($"LLM path escape detected: {path}");
    }
    private async Task ReadDirectory(DirectoryInfo directoryInfo, bool root = true, CancellationToken ct = default)
    {
        foreach (var dir in directoryInfo.GetDirectories())
        {
            if (root && dir.Name == "obj") continue;
            if (root && dir.Name == "bin") continue;
            if (root && dir.Name == ".vs") continue;
            await ReadDirectory(dir, false, ct);
        }

        foreach (var file in directoryInfo.GetFiles())
        {
            if (root && file.Name == "workspace.llm") continue;
            if (file.Extension is ".dll" or ".exe" or ".png" or ".jpg" or ".zip")
                continue;
            var relativePath = Path.GetRelativePath(RootDirectoryName, file.FullName);
            var fullPath = file.FullName;
            var workspaceFile = new WorkspaceFile(relativePath, fullPath);
            Files[relativePath] = workspaceFile;
        }
    }
}