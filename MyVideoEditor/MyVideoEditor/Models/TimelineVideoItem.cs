namespace MyVideoEditor.Models
{
    public class TimelineVideoItem
    {
        public TimelineVideoItem(Timeline timeline, VideoStreamReader videoStream)
        {
            Timeline = timeline;
            VideoStream = videoStream;
        }
        public Timeline Timeline { get; }
        public VideoStreamReader VideoStream { get; }

        public long StartFrame { get; set; }
        public long EndFrame { get; set; }
    }
}