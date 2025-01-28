namespace VideoEditor;

public class TimelineClip
{
    public TimelineClip(Timeline timeline, File file)
    {
        Timeline = timeline;
        File = file;
    }

    public Timeline Timeline { get; }
    public File File { get; }
    public int Layer { get; set; }
    public long TimelineStartIndex { get; set; }
    public long TimelineEndIndex { get; set; }
    public long ClipStartIndex { get; set; }
    public long ClipEndIndex { get; set; }
}