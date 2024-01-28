namespace MyVideoEditor.Models
{
    public class Timeline
    {
        public Timeline(Project project, string title)
        {
            Project = project;
            Title = title;
            VideoItems = new List<TimelineVideoItem>();
            AudioItems = new List<TimelineAudioItem>();
        }
        public Project Project { get; }
        public string Title { get; set; }
        public List<TimelineVideoItem> VideoItems { get; }
        public List<TimelineAudioItem> AudioItems { get; }

        public int FpsBase { get; set; }
        public int FpsDivider { get; set;}
        public Rectangle Resolution { get; set; }

        public double Fps => Convert.ToDouble(FpsBase) / FpsDivider;

        public void AddMedia(MediaContainer mediaContainer, double starttime)
        {

        }
    }
}