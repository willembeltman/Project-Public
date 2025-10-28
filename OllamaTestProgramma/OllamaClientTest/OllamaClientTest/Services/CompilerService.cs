namespace OllamaAgentGenerator.Services;

public class CompilerService
{
    private DirectoryInfo currentDirectory;

    public CompilerService(DirectoryInfo currentDirectory)
    {
        this.currentDirectory = currentDirectory;
    }

    /// <summary>
    /// Compiles anything in the currentDirectory
    /// So, tries to find a .sln or .csproj file (.sln has priority) to compile
    /// </summary>
    /// <returns>Compilation status (example: "No .sln or .csproj file was found" or "Build succesfull")</returns>
    public string Compile()
    {
        // Find file to compile
        // Compile it with process
        throw new NotImplementedException();
    }
}