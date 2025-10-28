namespace OllamaAgentGenerator.Services;

public class FileRepositoryService
{
    private readonly HashSet<string> _fileContents = new();
    private readonly DirectoryInfo _directoryInfo;

    public bool HasFiles => _fileContents.Count > 0;

    // Track all existing files and their content
    public FileRepositoryService(DirectoryInfo directoryInfo)
    {
        _directoryInfo = directoryInfo;
        InitializeFileTracking();
    }

    public void InitializeFileTracking()
    {
        _fileContents.Clear();

        if (!_directoryInfo.Exists)
            _directoryInfo.Create();

        ReadDirectory(_directoryInfo);
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
            _fileContents.Add($"[{file.Name}]:\n{content}\n");
        }
    }

    // Generate a text prompt showing all files and their contents
    public string GenerateFileContentsText()
    {
        if (_fileContents.Count == 0) return "<No files in directory>";
        return string.Join(Environment.NewLine, _fileContents);
    }

    // Generate MCP command template for the model to use
    public string GenerateMcpCommandsText()
    {
        return $@"
%%CreateOrUpdateFile(""<path>"", ""<content>"")%%
<path> is the relative path to the file, example: replace <path> with content\player.svg
<content> is the content of the file encoded as string, example replace <content> with <html>\n</html> 

%%MoveFile(""<oldPath>"", ""<newPath>"")%%
<oldPath> is the current path to the file, example: content\player.svg
<newPath> is the path you want to move the file to, example: content\player2.svg

%%DeleteFile(""<path>"")%%
<path> is the relative path of the file you want to delete, example: content\player.svg

Note: Please keep in mind to always use the %% for the beginning and the end(!)
Note #2: Please always replace <path>, <content>, <oldPath>, <newPath> and <path>";
    }

    // Process response containing MCP commands and execute them
    public bool ProcessResponse(string responseText)
    {
        var commandList = ParseMcpCommands(responseText).ToArray();
        if (commandList.Length == 0) return false;
        foreach (var command in commandList)
        {
            try
            {
                switch (command.Type)
                {
                    case "CreateOrUpdateFile":
                        CreateOrUpdateFile(command.Param1, command.Param2);
                        break;
                    case "MoveFile":
                        MoveFile(command.Param1, command.Param2);
                        break;
                    case "DeleteFile":
                        DeleteFile(command.Param1);
                        break;
                    default:
                        // Ignore unrecognized commands
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing command {command.Type}: {ex.Message}");
            }
        }
        return true;
    }

    private IEnumerable<(string Type, string Param1, string Param2)> ParseMcpCommands(string responseText)
    {
        var position = 0;
        var actionDetected = true;
        while (actionDetected)
        {
            var createPosition = responseText.IndexOf("%%CreateOrUpdateFile(", position);
            var movePosition = responseText.IndexOf("%%MoveFile(", position);
            var deletePosition = responseText.IndexOf("%%DeleteFile(", position);

            actionDetected = false;
            if (createPosition > 0)
            {
                actionDetected = true;
                var createPosition0 = responseText.IndexOf("(\"", createPosition) + 2;
                var createPosition1 = responseText.IndexOf("\",", createPosition0);
                var createPosition2 = responseText.IndexOf("\"", createPosition1 + 1) + 1;
                var createPosition3 = responseText.IndexOf("\")%%", createPosition2);
                var endPosition = createPosition3 + 4;
                if (position < endPosition)
                    position = endPosition;

                var fileName = responseText.Substring(createPosition0, createPosition1 - createPosition0);
                var content = responseText
                    .Substring(createPosition2, createPosition3 - createPosition2)
                    .Replace("\\r", "\r")
                    .Replace("\\n", "\n")
                    .Replace("\\\\", "\\");
                yield return ("CreateOrUpdateFile", fileName, content);
            }
            
            if (movePosition > 0)
            {
                actionDetected = true;
                var movePosition0 = responseText.IndexOf("(\"", movePosition) + 2;
                var movePosition1 = responseText.IndexOf("\",", movePosition0);
                var movePosition2 = responseText.IndexOf("\"", movePosition1 + 1) + 1;
                var movePosition3 = responseText.IndexOf("\")%%", movePosition2);
                var endPosition = movePosition3 + 4;
                if (position < endPosition)
                    position = endPosition;

                var oldPath = responseText.Substring(movePosition0, movePosition1 - movePosition0);
                var newPath = responseText.Substring(movePosition2, movePosition3 - movePosition2);
                yield return ("MoveFile", oldPath, newPath);
            }
            
            if (deletePosition > 0)
            {
                actionDetected = true;
                var deletePosition0 = responseText.IndexOf("(\"", deletePosition) + 2;
                var deletePosition1 = responseText.IndexOf("\")%%", deletePosition0);
                var endPosition = deletePosition1 + 4;
                if (position < endPosition)
                    position = endPosition;

                var path = responseText.Substring(deletePosition0, deletePosition1 - deletePosition0);
                yield return ("DeleteFile", path, string.Empty);
            }
        }
    }

    // File operation methods
    private void CreateOrUpdateFile(string path, string content)
    {
        var fullPath = Path.Combine(_directoryInfo.FullName, path);

        if (File.Exists(fullPath))
        {
            File.WriteAllText(fullPath, content);  // Overwrite existing file
            Console.WriteLine($"Updated {path}");
        }
        else
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!); // Ensure directory exists
            File.WriteAllText(fullPath, content);   // Create new file
            Console.WriteLine($"Created {path}");
        }
    }

    private void MoveFile(string path, string newPath)
    {
        if (File.Exists(path))
        {
            var newFullPath = Path.Combine(_directoryInfo.FullName, newPath);
            Directory.CreateDirectory(Path.GetDirectoryName(newFullPath)!); // Ensure target directory exists

            File.Move(path, newFullPath, true);     // Replace existing file if needed
            Console.WriteLine($"Moved {path} to {newPath}");
        }
    }

    private void DeleteFile(string path)
    {
        var fullPath = Path.Combine(_directoryInfo.FullName, path);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            Console.WriteLine($"Deleted {path}");
        }
    }
}

