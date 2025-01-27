
namespace VideoEditor;

public class TimelineVideoClip
{
    public TimelineVideoClip(
        Timeline timeline, 
        StreamInfoVideo videoStream, 
        double timelineStartInSeconds,
        double timelineEndInSeconds, 
        double videoStartInSeconds,
        double videoEndInSeconds) 
    {
        Timeline = timeline;
        VideoStream = videoStream;

        SetTimelineStartInSeconds(timelineStartInSeconds);
        SetTimelineEndInSeconds(timelineEndInSeconds);
        SetVideoStartInSeconds(videoStartInSeconds);
        SetVideoEndInSeconds(videoEndInSeconds);
    }

    public Timeline Timeline { get; set; }
    public StreamInfoVideo VideoStream { get; set; }

    public long TimelineStartIndex { get; set; }
    public double TimelineStartInSeconds => TimelineStartIndex * Timeline.VideoInfo.Fps.Divider / Timeline.VideoInfo.Fps.Base;
    private void SetTimelineStartInSeconds(double value)
    {
        TimelineStartIndex = Convert.ToInt64(value * Timeline.VideoInfo.Fps.Base / Timeline.VideoInfo.Fps.Divider);
    }

    public long TimelineEndIndex { get; set; }
    public double TimelineEndInSeconds => TimelineEndIndex * Timeline.VideoInfo.Fps.Divider / Timeline.VideoInfo.Fps.Base;
    private void SetTimelineEndInSeconds(double value)
    {
        TimelineEndIndex = Convert.ToInt64(value * Timeline.VideoInfo.Fps.Base / Timeline.VideoInfo.Fps.Divider);
    }

    public long VideoStartIndex { get; set; }
    public double VideoStartInSeconds => VideoStartIndex * Timeline.VideoInfo.Fps.Divider / Timeline.VideoInfo.Fps.Base;
    private void SetVideoStartInSeconds(double value)
    {
        VideoStartIndex = Convert.ToInt64(value * VideoStream.VideoInfo.Fps.Base / VideoStream.VideoInfo.Fps.Divider);
    }

    public long VideoEndIndex { get; set; }
    public double VideoEndInSeconds => VideoEndIndex * Timeline.VideoInfo.Fps.Divider / Timeline.VideoInfo.Fps.Base;
    private void SetVideoEndInSeconds(double value)
    {
        VideoEndIndex = Convert.ToInt64(value * VideoStream.VideoInfo.Fps.Base / VideoStream.VideoInfo.Fps.Divider);
    }
}

