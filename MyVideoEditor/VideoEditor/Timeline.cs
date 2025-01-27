namespace VideoEditor;

public class Timeline
{
    public Timeline(Project project)
    {
        Project = project;
    }

    public Project Project { get; }
    public VideoInfo VideoInfo { get; set; } = new VideoInfo();
    public ConcurrentArray<TimelineVideoClip> VideoClips { get; } = new ConcurrentArray<TimelineVideoClip>();
}
