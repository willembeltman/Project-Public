using VideoEditor.Enums;
using VideoEditor.Info;
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

    public static File[] TryOpenMultiple(IEnumerable<string> files)
    {
        var filteredfiles = CheckFileType.Filter(files);
        if (filteredfiles.Length == 0)
            return Array.Empty<File>();

        return filteredfiles
            .Select(a => new File(a))
            .ToArray();
    }

    public override string ToString()
    {
        return $"{FullName} {Duration}s";
    }
}
