using System.Diagnostics;

namespace LLMClient;

public static class ProcessHelper
{
    public static async Task<int> RunCommandAsync(string fileName, string arguments)
    {
        var psi = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var proc = new Process { StartInfo = psi };
        proc.Start();
        await proc.WaitForExitAsync();
        return proc.ExitCode;
    }
}
