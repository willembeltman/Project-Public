using MyCodingAgent.Services;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace MyCodingAgent;

public class Workspace
{
    public string RootDirectoryName { get; set; } = string.Empty;
    public string UserPrompt { get; set; } = string.Empty;
    public string? FileBrowserPath { get; set; }
    public Dictionary<string, WorkspaceFile> Files { get; set; } = new();
    public Dictionary<string, string> Notes { get; set; } = new();
    public Dictionary<string, string> Tasks { get; set; } = new();
    public List<AgentResponseResult> AgentResponseResults { get; set; } = new();
    public string? SearchText { get; set; }

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
        using var stream = File.OpenWrite(llmFileString);
        await JsonSerializer.SerializeAsync(stream, this);
    }
    public async Task<string> CompileAsync()
    {
        var currentDirectory = new DirectoryInfo(RootDirectoryName);
        var compileErrors = await Compiler.Compile(currentDirectory);
        return compileErrors
            .Replace(currentDirectory.FullName + "\\", "");
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
}