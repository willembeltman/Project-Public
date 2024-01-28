namespace MyVideoEditor.Models
{
    public class TimelineAudioItem
    {
        public TimelineAudioItem(Timeline timeline, AudioStreamReader videoStream)
        {
            Timeline = timeline;
            AudioStream = videoStream;
        }
        public Timeline Timeline { get; }
        public AudioStreamReader AudioStream { get; }

        public long StartFrame { get; set; }
        public long EndFrame { get; set; }
    }
}