using VideoEditor.Static;

namespace VideoEditor;

public class File
{
    public File(string fullName, IEnumerable<StreamInfo> streaminfos, double? duration)
    {
        var streams = new List<StreamInfo>();
        var videoStreams = new List<StreamInfoVideo>();
        var audioStreams = new List<StreamInfoAudio>();
        foreach (var info in streaminfos)
        {
            streams.Add(info);
            if (info.CodecType == CodecTypeEnum.Video)
            {
                videoStreams.Add(new StreamInfoVideo(this, info));
            }
            if (info.CodecType == CodecTypeEnum.Audio)
            {
                audioStreams.Add(new StreamInfoAudio(this, info));
            }
        }
        FullName = fullName;
        Duration = duration;
        Streams = streams.ToArray();
        VideoStreams = videoStreams.ToArray();
        AudioStreams = audioStreams.ToArray();
    }
    public string FullName { get; }
    public double? Duration { get; }
    public StreamInfo[] Streams { get; }
    public StreamInfoVideo[] VideoStreams { get; }
    public StreamInfoAudio[] AudioStreams { get; }

    public static File Open(string fullName)
    {
        var streaminfos = FFProbe.GetStreamInfos(fullName);
        var duration = FFProbe.GetDuration(fullName);
        return new File(fullName, streaminfos, duration);
    }
    public static File[] Open(IEnumerable<string> files)
    {
        var filteredfiles = CheckFileType.Filter(files);
        if (filteredfiles.Length == 0)
            return Array.Empty<File>();

        return filteredfiles
            .Select(a => Open(a))
            .ToArray();
    }
}
