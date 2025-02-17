using VideoEditor.Enums;
using VideoEditor.Static;

namespace VideoEditor;

public class File
{
    public File(string fullName)
    {
        var rapport = FFProbe.GetRapport(fullName);

        FullName = fullName;
        AllStreams =
            rapport.streams
                .Where(a => a.codec_type == "video" || a.codec_type == "audio")
                .Select(a => new StreamInfo(this, a))
                .ToArray();
        VideoStreams =
            AllStreams
                .Where(a => a.CodecType == CodecType.Video)
                .ToArray();
        AudioStreams =
            AllStreams
                .Where(a => a.CodecType == CodecType.Audio)
                .ToArray();
        Duration = FFHelpers.TryParseToDouble(rapport.format.duration, out var dur) ? dur : null;
    }

    public string FullName { get; }
    public StreamInfo[] AllStreams { get; }
    public StreamInfo[] VideoStreams { get; }
    public StreamInfo[] AudioStreams { get; }
    public double? Duration { get; }

    public static IEnumerable<File> OpenMultiple(IEnumerable<string> files)
    {
        return files
            .Select(a => new File(a));
    }

    public bool EqualTo(object? obj)
    {
        if (!(obj is File)) return false; 
        var other = obj as File;
        if (other == null) return false;
        if (FullName != other.FullName) return false;
        return true;
    }

    public override string ToString()
    {
        return $"{FullName} {Duration}s";
    }
}
