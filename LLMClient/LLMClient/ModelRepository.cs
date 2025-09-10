using System.Diagnostics;
using System.Text;

namespace LLMClient;

public static class ModelRepository
{
    public static async Task<string[]?> LoadModelListAsync()
    {
        var result = await RunCommandCaptureStdoutAsync("foundry", "model list");

        var lines = result.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var models = new List<string>();

        foreach (var line in lines)
        {
            // Sla header en scheidingslijnen over
            if (line.StartsWith("Alias") || line.StartsWith("---")) continue;

            // Splits de regel op whitespace
            var parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length > 0)
            {
                // laatste kolom is altijd het Model ID
                var modelId = parts[^1];
                models.Add(modelId);
            }
        }

        return models.ToArray();
    }

    private static async Task<string> RunCommandCaptureStdoutAsync(string fileName, string arguments)
    {
        var psi = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
            StandardOutputEncoding = Encoding.UTF8
        };

        using var proc = new Process { StartInfo = psi };
        var sb = new StringBuilder();
        proc.Start();
        using var sr = proc.StandardOutput;
        while (!sr.EndOfStream)
        {
            var line = await sr.ReadLineAsync();
            if (line != null)
            {
                sb.AppendLine(line);
            }
        }
        proc.WaitForExit();
        return sb.ToString();
    }
}

