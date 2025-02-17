namespace VideoEditor;

public class TimelineClipVideo : TimelineClip, ITimelineClip
{
    public TimelineClipVideo(Timeline timeline, StreamInfo streamInfo) : base(timeline, streamInfo)
    {
    }

    public double TimelineStartInSeconds
    {
        get => TimelineStartIndex * Timeline.Fps.Divider / Timeline.Fps.Base;
        set => TimelineStartIndex = Convert.ToInt64(value * Timeline.Fps.Base / Timeline.Fps.Divider);
    }
    public double TimelineEndInSeconds
    {
        get => TimelineEndIndex * Timeline.Fps.Divider / Timeline.Fps.Base;
        set => TimelineEndIndex = Convert.ToInt64(value * Timeline.Fps.Base / Timeline.Fps.Divider);
    }
    public double ClipStartInSeconds
    {
        get => ClipStartIndex * StreamInfo.Fps!.Value.Divider / StreamInfo.Fps!.Value.Base;
        set => ClipStartIndex = Convert.ToInt64(value * StreamInfo.Fps!.Value.Base / StreamInfo.Fps!.Value.Divider);
    }
    public double ClipEndInSeconds
    {
        get => ClipEndIndex * StreamInfo.Fps!.Value.Divider / StreamInfo.Fps!.Value.Base;
        set => ClipEndIndex = Convert.ToInt64(value * StreamInfo.Fps!.Value.Base / StreamInfo.Fps!.Value.Divider);
    }

    public bool IsVideoClip => true;
    public bool IsAudioClip => false;
}

