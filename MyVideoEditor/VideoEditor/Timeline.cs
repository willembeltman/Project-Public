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

    internal void AddFiles(double currentTime, int startlayer, IEnumerable<File> files)
    {
        foreach (var file in files)
        {
            if (file.Duration == null) continue;

            var start = currentTime;
            currentTime += file.Duration.Value;
            var layer = startlayer;
            foreach (var videoStream in file.VideoStreams)
            {
                VideoClips.Add(
                    new TimelineVideoClip(this, file, videoStream)
                    {
                        Layer = layer,
                        TimelineStartInSeconds = start,
                        TimelineEndInSeconds = currentTime,
                        ClipStartInSeconds = 0,
                        ClipEndInSeconds = file.Duration.Value
                    });
                layer++;
            }
            layer = 0;
            foreach (var audioStream in file.AudioStreams)
            {
                AudioClips.Add(
                    new TimelineAudioClip(this, file, audioStream)
                    {
                        Layer = layer,
                        TimelineStartInSeconds = start,
                        TimelineEndInSeconds = currentTime,
                        ClipStartInSeconds = 0,
                        ClipEndInSeconds = file.Duration.Value
                    });
                layer++;
            }
            Project.Files.Add(file);
        }
    }
}
