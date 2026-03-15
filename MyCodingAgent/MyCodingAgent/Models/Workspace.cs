using MyCodingAgent.Compile;
using MyCodingAgent.Helpers;
using System.Text;
using System.Text.Json;

namespace MyCodingAgent.Models;

public class Workspace
{
    public int PromptIndex { get; set; }
    public string RootDirectoryName { get; set; } = string.Empty;
    public string UserPrompt { get; set; } = string.Empty;
    public bool UserPromptDone { get; set; }
    public List<WorkspaceFile> Files { get; set; } = [];
    public List<AgentResponseResult> CodingHistory { get; set; } = [];
    public List<AgentResponseResult> DebugHistory { get; set; } = [];

    public WorkspaceFile? GetFile(string path)
        => Files.FirstOrDefault(a => a.RelativePath.Equals(path.Replace("/", "\\"), StringComparison.CurrentCultureIgnoreCase));
  
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

                // For when the developer has changed the source code
                await workspace.InitializeDirectory(rootDirectory); 
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

        // For when the developer has already setup a project and the workspace.llm file is just missing
        await workspace.InitializeDirectory(rootDirectory);
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
        
    private async Task InitializeDirectory(DirectoryInfo directoryInfo, bool isRoot = true, CancellationToken ct = default)
    {
        foreach (var dir in directoryInfo.GetDirectories())
        {
            if (isRoot && dir.Name == "obj") continue;
            if (isRoot && dir.Name == "bin") continue;
            if (isRoot && dir.Name == ".vs") continue;
            await InitializeDirectory(dir, false, ct);
        }

        foreach (var file in directoryInfo.GetFiles())
        {
            if (isRoot && file.Name == "workspace.llm") continue;
            if (file.Extension is ".dll" or ".exe" or ".png" or ".jpg" or ".zip")
                continue;
            var relativePath = Path.GetRelativePath(RootDirectoryName, file.FullName);
            var fullPath = file.FullName;
            var workspaceFile = new WorkspaceFile(relativePath, fullPath);
            Files.Add(workspaceFile);
        }
    }
    public void TryParseFullPath(string path, out string fullPath)
    {
        var currentDirectory = new DirectoryInfo(RootDirectoryName);

        fullPath = Path.GetFullPath(
            Path.Combine(currentDirectory.FullName, path));

        if (!fullPath.StartsWith(currentDirectory.FullName + Path.DirectorySeparatorChar))
            throw new Exception($"LLM path escape detected: {path}");
    }

    public async Task<string> GetListAllFilesText()
    {
        StringBuilder sb = new StringBuilder();
        if (Files.Count > 0)
        {
            foreach (var file in Files)
            {
                var fileContent = await file.GetFileContent();
                sb.AppendLine($"{file.RelativePath} ({fileContent.GetLineCount()} lines)");
            }
        }
        else
        {
            sb.AppendLine("<No files found in project>");
        }
        return sb.ToString();
    }
}