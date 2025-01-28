namespace VideoEditor
{
    internal interface ITimelineClip
    {
        Timeline Timeline { get; }
        File File { get; }
        int Layer { get; set; }
        long TimelineStartIndex { get; set; }
        long TimelineEndIndex { get; set; }
        long ClipStartIndex { get; set; }
        long ClipEndIndex { get; set; }
        double TimelineStartInSeconds { get; set; }
        double TimelineEndInSeconds { get; set; }
        double ClipStartInSeconds { get; set; }
        double ClipEndInSeconds { get; set; }
    }
}