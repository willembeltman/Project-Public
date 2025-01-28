using VideoEditor.Info;
namespace VideoEditor;

public class TimelineVideoClip : TimelineClip, ITimelineClip
{
    public TimelineVideoClip(Timeline timeline, File file, StreamInfo videoStream) : base(timeline, file)
    {
        VideoStream = videoStream;
    }

    public StreamInfo VideoStream { get; }

    public double TimelineStartInSeconds
    {
        get => TimelineStartIndex * Timeline.VideoInfo.Fps.Divider / Timeline.VideoInfo.Fps.Base;
        set => TimelineStartIndex = Convert.ToInt64(value * Timeline.VideoInfo.Fps.Base / Timeline.VideoInfo.Fps.Divider);
    }
    public double TimelineEndInSeconds
    {
        get => TimelineEndIndex * Timeline.VideoInfo.Fps.Divider / Timeline.VideoInfo.Fps.Base;
        set => TimelineEndIndex = Convert.ToInt64(value * Timeline.VideoInfo.Fps.Base / Timeline.VideoInfo.Fps.Divider);
    }
    public double ClipStartInSeconds
    {
        get => ClipStartIndex * VideoStream.Fps.Divider / VideoStream.Fps.Base;
        set => ClipStartIndex = Convert.ToInt64(value * VideoStream.Fps.Base / VideoStream.Fps.Divider);
    }
    public double ClipEndInSeconds
    {
        get => ClipEndIndex * VideoStream.Fps.Divider / VideoStream.Fps.Base;
        set => ClipEndIndex = Convert.ToInt64(value * VideoStream.Fps.Base / VideoStream.Fps.Divider);
    }
}

