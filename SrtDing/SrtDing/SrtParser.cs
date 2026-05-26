using System.Globalization;

public static class SrtParser
{
    public static List<SubtitleRecord> Parse(string path)
    {
        var lines = File.ReadAllLines(path);

        var result = new List<SubtitleRecord>();

        int i = 0;

        while (i < lines.Length)
        {
            // skip lege regels
            while (i < lines.Length && string.IsNullOrWhiteSpace(lines[i]))
            {
                i++;
            }

            if (i >= lines.Length)
            {
                break;
            }

            // index
            var index = int.Parse(lines[i]);
            i++;

            // timing
            var timing = lines[i];
            i++;

            var split = timing.Split(" --> ");

            var start = ParseTime(split[0]);
            var end = ParseTime(split[1]);

            // text
            var textLines = new List<string>();

            while (i < lines.Length && !string.IsNullOrWhiteSpace(lines[i]))
            {
                textLines.Add(lines[i]);
                i++;
            }

            result.Add(new SubtitleRecord(
                index,
                start,
                end,
                textLines));
        }

        return result;
    }

    private static TimeSpan ParseTime(string value)
    {
        return TimeSpan.ParseExact(
            value,
            @"hh\:mm\:ss\,fff",
            CultureInfo.InvariantCulture);
    }
}

public record SubtitleRecord(
    int Index,
    TimeSpan Start,
    TimeSpan End,
    List<string> Lines)
{
    public string Text => string.Join(Environment.NewLine, Lines);
}