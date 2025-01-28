using VideoEditor.Info;

namespace VideoEditor;

public class Timeline
{
    public Timeline(Project project)
    {
        Project = project;
    }

    public Project Project { get; }

    public ConcurrentArray<TimelineVideoClip> VideoClips { get; } = new ConcurrentArray<TimelineVideoClip>();
    public ConcurrentArray<TimelineAudioClip> AudioClips { get; } = new ConcurrentArray<TimelineAudioClip>();

    public AudioInfo AudioInfo { get; } = new AudioInfo();
    public VideoInfo VideoInfo { get; } = new VideoInfo();
}
