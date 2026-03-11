using MyCodingAgent.Services;

namespace MyCodingAgent;

public class Workspace(string currentDirectoryName)
{
    public readonly DirectoryInfo CurrentDirectory = new(currentDirectoryName);
    public readonly Dictionary<string, string> FileRepository = new();
    public readonly Dictionary<int, string> Memory = new();
    public readonly Dictionary<int, string> Tasks = new();
    public readonly Queue<string> McpHistory = new();
    public string CompileErrors = string.Empty;

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
        CompileErrors = compileErrors
            .Replace(CurrentDirectory.FullName + "\\", "");
    }

    // Process response containing MCP commands and execute them
    public bool ProcessResponse(string responseText)
    {
        var found = false;
        foreach (var action in ParseActions(responseText))
        {
            found = true;
            switch (action.Type)
            {
                case "create_or_update_file":
                    CreateOrUpdateFile(action.Path!, action.Content!);
                    break;

                case "delete_file":
                    DeleteFile(action.Path!);
                    break;

                case "move_file":
                    MoveFile(action.OldPath!, action.NewPath!);
                    break;

                // Todo de rest

                // Ps: compile hoeft niet

                default:
                    found = false;
                    break;
            }
        }
        return found;
    }

    private IEnumerable<AgentAction> ParseActions(string responseText)
    {
        throw new NotImplementedException();
    }

    // MCP INTERFACE
    private void CreateDirectory(string path)
    {
        if (!CheckPath(path)) 
            throw new Exception($"LLLM tries to hack me! WTF? gives a path {path}");
        var fullPath = Path.Combine(CurrentDirectory.FullName, path);
        Directory.CreateDirectory(fullPath);
    }
    private void RemoveDirectory(string path)
    {
        if (!CheckPath(path))
            throw new Exception($"LLLM tries to hack me! WTF? gives a path {path}");
        var fullPath = Path.Combine(CurrentDirectory.FullName, path);
        Directory.Delete(fullPath);
    }

    private void CreateOrUpdateFile(string path, string fullContent)
    {
        var fullPath = Path.Combine(CurrentDirectory.FullName, path);

        if (File.Exists(fullPath))
        {
            File.WriteAllText(fullPath, fullContent);  // Overwrite existing file
            Console.WriteLine($"Updated {path}");
        }
        else
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!); // Ensure directory exists
            File.WriteAllText(fullPath, fullContent);   // Create new file
            Console.WriteLine($"Created {path}");
        }
    }
    private void PartialOverwriteFile(string path, int startLineNr, int endLineNr, string fullContent)
    {

    }
    private void MoveFile(string path, string newPath)
    {
        if (File.Exists(path))
        {
            var newFullPath = Path.Combine(CurrentDirectory.FullName, newPath);
            Directory.CreateDirectory(Path.GetDirectoryName(newFullPath)!); // Ensure target directory exists

            File.Move(path, newFullPath, true);     // Replace existing file if needed
            Console.WriteLine($"Moved {path} to {newPath}");
        }
    }
    private void DeleteFile(string path)
    {
        var fullPath = Path.Combine(CurrentDirectory.FullName, path);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            Console.WriteLine($"Deleted {path}");
        }
    }

    private void AddMemory(int id, string content)
    {
        throw new NotImplementedException();
    }
    private void DeleteMemory(int id, string content)
    {
        throw new NotImplementedException();
    }
    private void AddTask(int id, string content)
    {
        throw new NotImplementedException();
    }
    private void DeleteTask(int id, string content)
    {
        throw new NotImplementedException();
    }

    private bool CheckPath(string path)
    {
        // Stay within workspace ;)
        throw new NotImplementedException();
    }

    private void ReadDirectory(DirectoryInfo directoryInfo)
    {
        foreach (var dir in directoryInfo.GetDirectories())
        {
            ReadDirectory(dir);
        }

        foreach (var file in directoryInfo.GetFiles())
        {
            using var reader = new StreamReader(file.FullName);
            string content = reader.ReadToEnd();
            FileRepository[file.Name] = content;
        }
    }
}

