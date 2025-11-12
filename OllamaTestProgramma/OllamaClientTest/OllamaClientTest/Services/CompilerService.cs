using System.Diagnostics;

namespace OllamaAgentGenerator.Services;

using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

public class CompilerService
{
    private readonly DirectoryInfo currentDirectory;

    public CompilerService(DirectoryInfo currentDirectory)
    {
        this.currentDirectory = currentDirectory ?? throw new ArgumentNullException(nameof(currentDirectory));
    }

    /// <summary>
    /// Compiles the solution or project in the current directory.
    /// Returns a detailed report including errors and warnings if any.
    /// </summary>
    public string Compile()
    {
        var slnFile = currentDirectory.GetFiles("*.sln", SearchOption.TopDirectoryOnly).FirstOrDefault();
        var csprojFile = currentDirectory.GetFiles("*.csproj", SearchOption.TopDirectoryOnly).FirstOrDefault();

        if (slnFile == null && csprojFile == null)
            return "No .sln or .csproj file was found.";

        string fileToBuild = slnFile?.FullName ?? csprojFile.FullName;

        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"build \"{fileToBuild}\" -nologo -v:m",
            WorkingDirectory = currentDirectory.FullName,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        try
        {
            using var process = new Process { StartInfo = startInfo };
            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();

            process.OutputDataReceived += (_, e) => { if (e.Data != null) outputBuilder.AppendLine(e.Data); };
            process.ErrorDataReceived += (_, e) => { if (e.Data != null) errorBuilder.AppendLine(e.Data); };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            string output = outputBuilder.ToString();
            string errors = errorBuilder.ToString();

            // Try to extract compiler errors using regex
            // Example line: "Program.cs(10,5): error CS1002: ; expected [MyApp -> MyApp.csproj]"
            var matches = Regex.Matches(output + errors, @"^(?<file>.+?)\((?<line>\d+),(?<col>\d+)\):\s*(?<type>error|warning)\s*(?<code>CS\d+):\s*(?<msg>.+)$",
                RegexOptions.Multiline);

            if (matches.Count > 0)
            {
                var formatted = new StringBuilder();
                formatted.AppendLine($"Build failed ({matches.Count} issue{(matches.Count == 1 ? "" : "s")}):");

                foreach (Match m in matches)
                {
                    formatted.AppendLine($"[{m.Groups["type"].Value.ToUpper()} {m.Groups["code"].Value}] {m.Groups["file"].Value}({m.Groups["line"].Value},{m.Groups["col"].Value}): {m.Groups["msg"].Value}");
                }

                return formatted.ToString();
            }

            if (process.ExitCode == 0)
                return $"Build successful.\n{output.Trim()}";

            // No regex matches but nonzero exit: return raw logs
            return $"Build failed (exit code {process.ExitCode}).\n{output}\n{errors}";
        }
        catch (Exception ex)
        {
            return $"Build process threw an exception: {ex.Message}";
        }
    }
}

