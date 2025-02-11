namespace VideoEditor;

public class TimelineClip
{
    public TimelineClip(Timeline timeline, StreamInfo streamInfo, int layer)
    {
        Timeline = timeline;
        StreamInfo = streamInfo;
        Layer = layer;
    }

    public Timeline Timeline { get; }
    public StreamInfo StreamInfo { get; }
    public int Layer { get; set; }
    public long TimelineStartIndex { get; set; }
    public long TimelineEndIndex { get; set; }
    public long ClipStartIndex { get; set; }
    public long ClipEndIndex { get; set; }
}