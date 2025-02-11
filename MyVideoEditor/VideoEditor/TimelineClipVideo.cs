﻿namespace VideoEditor;

public class TimelineClipVideo : TimelineClip, ITimelineClip
{
    public TimelineClipVideo(Timeline timeline, StreamInfo streamInfo, double clipStart, int layer) : base(timeline, streamInfo, layer)
    {
        ClipStartInSeconds = clipStart;
    }

    public double TimelineStartInSeconds
    {
        get => TimelineStartIndex * Timeline.Info.Fps.Divider / Timeline.Info.Fps.Base;
        set => TimelineStartIndex = Convert.ToInt64(value * Timeline.Info.Fps.Base / Timeline.Info.Fps.Divider);
    }
    public double TimelineEndInSeconds
    {
        get => TimelineEndIndex * Timeline.Info.Fps.Divider / Timeline.Info.Fps.Base;
        set => TimelineEndIndex = Convert.ToInt64(value * Timeline.Info.Fps.Base / Timeline.Info.Fps.Divider);
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

    public bool IsVideoClip => true;
    public bool IsAudioClip => false;
}

