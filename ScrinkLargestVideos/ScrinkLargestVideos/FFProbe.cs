using System.Diagnostics;
using System.Text.Json;

namespace ScrinkLargestVideos;

public static class FFProbe
{
    public static FFProbeRapport GetRapport(FileInfo info)
    {
        string json = GetRapportJson(info.FullName);
        return Deserialize(json);
    }

    public static FFProbeRapport Deserialize(string json)
    {
        return JsonSerializer.Deserialize<FFProbeRapport>(json)
            ?? throw new Exception("Error reading file");
    }

    public static string GetRapportJson(string fullName)
    {
        var arguments = $" -v error -show_format -show_streams -print_format json \"{fullName}\"";

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = FFExecutebles.FFProbe.FullName,
                WorkingDirectory = FFExecutebles.FFProbe.Directory?.FullName,
                Arguments = arguments,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        string json = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return json;
    }
}
