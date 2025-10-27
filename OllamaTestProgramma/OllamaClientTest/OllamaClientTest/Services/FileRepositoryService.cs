
namespace OllamaAgentGenerator.Services;

// Please finish this class, be mindfull of the encoding of both commands and 
public class FileRepositoryService(DirectoryInfo directoryInfo)
{
    public bool WantsToSeeCompile { get; internal set; }

    public string GenerateFileContentsText()
    {
        // Give a list of all files inside the directory (recursive) with their contents
        // Example:
        // index.html
        // <html>
        // </html>
        // index.css
        // html { background: black; }
        throw new NotImplementedException();
    }
    public string GenerateMcpCommandsText()
    {
        // Give a list of command the llm can callback:
        // Example: 
        // CreateOrUpdateFile(path, content)
        // MoveFile(path, newPath)
        // DeleteFile(path)
        // Compile()
        throw new NotImplementedException();
    }
    public void ProcessResponse(string responseText)
    {
        WantsToSeeCompile = false;
        if (responseText.Contains("CreateOrUpdateFile"))
            CreateOrUpdateFile("todo", "todo");
        else if (responseText.Contains("MoveFile"))
            MoveFile("todo", "todo");
        else if (responseText.Contains("DeleteFile"))
            DeleteFile("todo");
        else if (responseText.Contains("Compile"))
            WantsToSeeCompile = true;
    }

    private void CreateOrUpdateFile(string path, string content)
    {

    }
    private void MoveFile(string path, string newPath)
    {

    }
    private void DeleteFile(string path)
    {

    }
}
