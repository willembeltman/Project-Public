using MyCodingAgent.Compile;
using System.Text.Json;

namespace MyCodingAgent;

public class Workspace
{
    public string RootDirectoryName { get; set; } = string.Empty;
    public string UserPrompt { get; set; } = string.Empty;
    public bool UserPromptDone { get; set; }
    public string? CurrentOpenFile { get; set; }
    public Dictionary<string, WorkspaceFile> Files { get; set; } = new();
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
        var compileErrors = await Compiler.Compile(currentDirectory);
        return compileErrors;
    }
    public void TryParseFullPath(string path, out string fullPath)
    {
        var currentDirectory = new DirectoryInfo(RootDirectoryName);
        fullPath = Path.GetFullPath(
            Path.Combine(currentDirectory.FullName, path));

        if (!fullPath.StartsWith(currentDirectory.FullName))
            throw new Exception($"LLM tries to hack me: {path}");
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
            var fileContent = await File.ReadAllTextAsync(file.FullName, ct);
            var relativePath = Path.GetRelativePath(RootDirectoryName, file.FullName);
            Files[relativePath] = new WorkspaceFile(relativePath, file.FullName, fileContent);
        }
    }

    public async Task<string> Search(string searchText)
    {
        SearchText = searchText;
        return $"Changed search text to: '{searchText}'";
    }

    public async Task<string> OpenFile(string path)
    {
        if (Files.TryGetValue(path, out _))
        {
            CurrentOpenFile = path;
            return $"Opened workspace file '{path}'";
        }
        else
        {
            return $"Tried to open workspace file '{path}': Error, file not found";
        }
    }

    public async Task<string> CreateDirectory(string path)
    {
        TryParseFullPath(path, out var fullPath);
        try
        {
            Directory.CreateDirectory(fullPath);
            return $"Created directory '{path}'";
        }
        catch (Exception ex)
        {
            return $"Tried to created directory '{path}': Error {ex}";
        }
    }

    public async Task<string> RemoveDirectory(string path)
    {
        TryParseFullPath(path, out var fullPath);
        try
        {
            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true);
                return $"Removed directory '{path}'";
            }
            else
            {
                return $"Tried to remove directory '{path}': Error directory does not exist";
            }
        }
        catch (Exception ex)
        {
            return $"Tried to remove directory '{path}': Error {ex}";
        }
    }

    public async Task<string> MoveFile(string path, string newPath)
    {
        TryParseFullPath(path, out var fullPath);
        TryParseFullPath(newPath, out var newFullPath);

        if (File.Exists(fullPath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(newFullPath)!);

            File.Move(fullPath, newFullPath, true);
            Files.Remove(path);

            var content = await File.ReadAllTextAsync(newFullPath);
            Files[newPath] =
                new WorkspaceFile(path, fullPath, content);

            return $"Moved {path} -> {newPath}";
        }
        return $"Error: could not find '{path}'";
    }

    public async Task<string> DeleteFile(string path)
    {
        TryParseFullPath(path, out var fullPath);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            Files.Remove(path);

            return $"Deleted '{path}'";
        }
        return $"Error: could not find '{path}'";
    }

    public async Task<string> CreateOrUpdateFile(string path, string fullContent)
    {
        TryParseFullPath(path, out var fullPath);
        var fileInfo = new FileInfo(fullPath);
        if (fileInfo.Directory == null)
            throw new Exception($"Weird stuff, directory is empty? {fileInfo}");
        if (fileInfo.Directory.Exists == false)
            fileInfo.Directory.Create();

        await File.WriteAllTextAsync(fullPath, fullContent);

        Files[path] =
            new WorkspaceFile(path, fullPath, fullContent);

        return $"Updated {path}";
    }

    public async Task<string> PartialOverwriteFile(string path, int startLineNr, int endLineNr, string newContent)
    {
        try
        {
            TryParseFullPath(path, out var fullPath);
            var file = Files[path];
            var lines = file.FileContent.Split('\n').ToList();
            var newLines = newContent.Split('\n');

            lines.RemoveRange(startLineNr - 1, endLineNr - startLineNr + 1);
            lines.InsertRange(startLineNr - 1, newLines);

            var content = string.Join("\n", lines);

            await File.WriteAllTextAsync(fullPath, content);

            Files[path] =
                new WorkspaceFile(path, fullPath, content);

            return $"Updated '{path}'";
        }
        catch (Exception ex)
        {
            return $"Tried to update '{path}', startLine {startLineNr}, endLineNr {endLineNr}: Error {ex.Message}";
        }
    }

    public async Task<string> FindAndReplace(string path, string searchText, string replaceText)
    {
        SearchText = searchText;
        return $"Changed search text to: '{searchText}'";
    }

    public async Task<string> FindAndReplaceAll(string searchText, string replaceText)
    {
        SearchText = searchText;
        return $"Changed search text to: '{searchText}'";
    }

    public async Task<string> CreateOrUpdateTask(string id, string content)
    {
        Tasks[id] = content;
        return $"Updated task '{id}'";
    }

    public async Task<string> DeleteTask(string id)
    {
        Tasks.Remove(id);
        return $"Removed task '{id}'";
    }

}