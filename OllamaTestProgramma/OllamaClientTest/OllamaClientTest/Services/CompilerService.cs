using System.Diagnostics;

namespace OllamaAgentGenerator.Services;

public class CompilerService
{
    private readonly DirectoryInfo currentDirectory;

    public CompilerService(DirectoryInfo currentDirectory)
    {
        this.currentDirectory = currentDirectory ?? throw new ArgumentNullException(nameof(currentDirectory));
    }

    /// <summary>
    /// Compiles anything in the currentDirectory.
    /// Looks for a .sln first, then a .csproj.
    /// Returns a status string (e.g. "Build successful" or error details).
    /// </summary>
    /// <returns>Compilation status.</returns>
    public string Compile()
    {
        // 1. Find a file to compile
        var slnFile = currentDirectory.GetFiles("*.sln", SearchOption.TopDirectoryOnly).FirstOrDefault();
        var csprojFile = currentDirectory.GetFiles("*.csproj", SearchOption.TopDirectoryOnly).FirstOrDefault();

        if (slnFile == null && csprojFile == null)
        {
            return "No .sln or .csproj file was found.";
        }

        // .sln takes precedence
        string fileToBuild = slnFile?.FullName ?? csprojFile.FullName;

        // 2. Prepare the process
        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"build \"{fileToBuild}\"",
            WorkingDirectory = currentDirectory.FullName,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        try
        {
            using var process = Process.Start(startInfo);
            if (process == null)
                return "Failed to start build process.";

            // Capture the output
            string stdout = process.StandardOutput.ReadToEnd();
            string stderr = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (process.ExitCode == 0)
            {
                return $"Build successful.\n{stdout}";
            }
            else
            {
                return $"Build failed (exit code {process.ExitCode}).\n{stderr}";
            }
        }
        catch (Exception ex)
        {
            // If the dotnet CLI is not available or another error occurs
            return $"Build process threw an exception: {ex.Message}";
        }
    }
}
