namespace VideoEditor;

public class StreamInfoVideo
{
    public StreamInfoVideo(File file, StreamInfo info)
    {
        File = file; 
        Index = info.Index;
        VideoInfo = new VideoInfo(info.Resolution, info.Fps);
    }

    public File File { get; }
    public int Index { get; }
    public VideoInfo VideoInfo { get; }
}

