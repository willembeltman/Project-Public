namespace MyVideoEditor.Models
{
    public class Timeline
    {
        public List<MediaContainer> Medias { get; set; } = new List<MediaContainer>();
        public List<TimelineVideoItem> VideoItems { get; set; } = new List<TimelineVideoItem>();
    }

    public class TimelineVideoItem
    {
    }
}