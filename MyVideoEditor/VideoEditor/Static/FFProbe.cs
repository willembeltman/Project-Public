using System.Diagnostics;

namespace VideoEditor.Static;

public static class FFProbe
{
    public static double? GetDuration(string fullName)
    {
        var args = " -v error -show_entries format=duration -v quiet -of csv=\"p=0\" \"" + fullName + "\"";
        double? duration = null;
        var lines = ReadLines(args);
        foreach (var line2 in lines)
        {
            if (!string.IsNullOrEmpty(line2))
            {
                if (double.TryParse(ReplaceNumber(line2), out var dur))
                {
                    duration = dur;
                    break;
                }
            }
        }
        return duration;
    }

    public static StreamInfo[] GetStreamInfos(string fullName)
    {
        var args = " -v error -show_entries stream=codec_long_name,codec_type,width,height,r_frame_rate,duration,bit_rate -of csv=s=,:p=0 \"" + fullName + "\"";
        var streamindex = 0;
        var streaminfos = new List<StreamInfo>();
        var lines = ReadLines(args);
        foreach (var line in lines)
        {
            var parameters = line.Split([","], StringSplitOptions.None);
            if (parameters.Length <= 1) { continue; }

            var streaminfo = new StreamInfo
            {
                Index = streamindex,
                Codec = parameters[0],
                CodecTypeString = parameters[1]
            };

            if (parameters.Length == 5)
            {
                if (double.TryParse(ReplaceNumber(parameters[3]), out double duration))
                    streaminfo.Duration = duration;

                if (int.TryParse(parameters[4], out int bitrate))
                    streaminfo.Bitrate = bitrate;
            }

            if (parameters.Length == 7)
            {
                if (int.TryParse(parameters[2], out int width) &&
                    int.TryParse(parameters[3], out int height))
                    streaminfo.Resolution = new Size(width, height);

                var fpsparameters = parameters[4].Split(["/"], StringSplitOptions.None);
                if (fpsparameters.Length == 2)
                {
                    if (long.TryParse(fpsparameters[0], out long fpsbase))
                        streaminfo.Fps.Base = fpsbase;
                    if (long.TryParse(fpsparameters[1], out long fpsdivider))
                        streaminfo.Fps.Divider = fpsdivider;
                }

                if (double.TryParse(ReplaceNumber(parameters[5]), out double duration))
                    streaminfo.Duration = duration;

                if (int.TryParse(parameters[6], out int bitrate))
                    streaminfo.Bitrate = bitrate;
            }

            streaminfos.Add(streaminfo);
            streamindex++;
        }

        return streaminfos.ToArray();
    }

    private static IEnumerable<string> ReadLines(string args)
    {
        var startinfo = new ProcessStartInfo()
        {
            FileName = FFExecuteblesPaths.FFProbe.FullName,
            WorkingDirectory = FFExecuteblesPaths.FFProbe.Directory?.FullName,
            Arguments = args,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };

        using (var cmd = new Process() { StartInfo = startinfo })
        {
            cmd.Start();

            var reader = cmd.StandardOutput;
            do
            {
                var line = reader.ReadLine();
                if (line != null)
                {
                    yield return line;
                }
            } while (!reader.EndOfStream);
        }
    }

    private static string ReplaceNumber(string text)
    {
        return text
            .Replace(".", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator)
            .Replace(",", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
    }
}
