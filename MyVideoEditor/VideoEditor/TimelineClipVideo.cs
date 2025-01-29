namespace VideoEditor;

public class TimelineClipVideo : TimelineClip, ITimelineClip
{
    public TimelineClipVideo(Timeline timeline, StreamInfo streamInfo) : base(timeline, streamInfo)
    {
    }

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
        get => ClipStartIndex * StreamInfo.Fps!.Divider / StreamInfo.Fps!.Base;
        set => ClipStartIndex = Convert.ToInt64(value * StreamInfo.Fps!.Base / StreamInfo.Fps!.Divider);
    }
    public double ClipEndInSeconds
    {
        get => ClipEndIndex * StreamInfo.Fps!.Divider / StreamInfo.Fps!.Base;
        set => ClipEndIndex = Convert.ToInt64(value * StreamInfo.Fps!.Base / StreamInfo.Fps!.Divider);
    }
}

