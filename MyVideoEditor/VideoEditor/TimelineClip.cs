namespace VideoEditor;

public class TimelineClip
{
    public TimelineClip(Timeline timeline, StreamInfo streamInfo)
    {
        Timeline = timeline;
        StreamInfo = streamInfo;
    }

    public Timeline Timeline { get; }
    public StreamInfo StreamInfo { get; }
    public int Layer { get; set; }
    public long TimelineStartIndex { get; set; }
    public long TimelineEndIndex { get; set; }
    public long ClipStartIndex { get; set; }
    public long ClipEndIndex { get; set; }
    public double OldTimelineStartInSeconds { get; set; }
    public double OldTimelineEndInSeconds { get; set; }
    public int OldLayer { get; set; }
}