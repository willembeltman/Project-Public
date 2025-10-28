namespace OllamaAgentGenerator.Services;

internal class CompilerService
{
    private DirectoryInfo currentDirectory;

    public CompilerService(DirectoryInfo currentDirectory)
    {
        this.currentDirectory = currentDirectory;
    }

    internal string Compile()
    {
        throw new NotImplementedException();
    }
}