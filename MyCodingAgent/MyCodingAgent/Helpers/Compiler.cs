using MyCodingAgent.Models;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace MyCodingAgent.Helpers;

public static class Compiler
{
    public static async Task<CompileResult> Compile(DirectoryInfo currentDirectory)
    {
        var slnFile = currentDirectory.GetFiles("*.sln", SearchOption.TopDirectoryOnly).FirstOrDefault();
        var csprojFile = currentDirectory.GetFiles("*.csproj", SearchOption.TopDirectoryOnly).FirstOrDefault();
        var fileToBuild = slnFile?.FullName ?? csprojFile?.FullName;
        if (fileToBuild == null)
            return new CompileResult()
            {
                Output = "No .sln or .csproj file was found.",
                Errors = [
                    new CompileError("No .sln or .csproj file was found.")
                ]
            };

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


        var result = new CompileResult();

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
                // Errors matched
                var formatted = new StringBuilder();
                formatted.AppendLine($"Build failed ({matches.Count} issue{(matches.Count == 1 ? "" : "s")}):");

                foreach (Match m in matches)
                {
                    var fullError = $"[{m.Groups["type"].Value.ToUpper()} {m.Groups["code"].Value}] {m.Groups["file"].Value}({m.Groups["line"].Value},{m.Groups["col"].Value}): {m.Groups["msg"].Value}"
                        .Replace(currentDirectory.FullName + "\\", "");
                    var error = new CompileError(
                        fullError,
                        m.Groups["type"].Value,
                        m.Groups["code"].Value
                            .Replace(currentDirectory.FullName + "\\", ""),
                        m.Groups["file"].Value
                    .Replace(currentDirectory.FullName + "\\", ""),
                        m.Groups["line"].Value,
                        m.Groups["col"].Value,
                        m.Groups["msg"].Value
                            .Replace(currentDirectory.FullName + "\\", ""));

                    formatted.AppendLine(fullError);
                    if (error.Type?.ToLower() == "warning")
                        result.Warnings.Add(error);
                    else
                        result.Errors.Add(error);
                }

                result.Output = formatted
                    .ToString();
            }
            else if (process.ExitCode == 0)
            {
                // Build succesful
                var formatted = output
                    .Trim()
                    .Replace(currentDirectory.FullName + "\\", "");
                result.Output = $"Build successful.\n{formatted}";
            }
            else
            {
                // No regex matches but nonzero exit: return raw logs
                var formatted = $"Build failed (exit code {process.ExitCode}).\n{output}\n{errors}"
                    .Replace(currentDirectory.FullName + "\\", "");

                result.Output = formatted;
                result.Errors.Add(new CompileError(formatted));
            }
        }
        catch (Exception ex)
        {
            var formatted = $"Build process threw an exception: {ex.Message}"
                .Replace(currentDirectory.FullName + "\\", "");

            result.Output = formatted;
            result.Errors.Add(new CompileError(formatted));
        }

        return result;
    }
}
