namespace VideoEditor;

public class Timeline
{
    public Timeline(Project project)
    {
        Project = project;
    }

    public Project Project { get; }

    public ConcurrentArray<TimelineClipVideo> VideoClips { get; } = new ConcurrentArray<TimelineClipVideo>();
    public ConcurrentArray<TimelineClipAudio> AudioClips { get; } = new ConcurrentArray<TimelineClipAudio>();

    public TimelineInfo VideoInfo { get; } = new TimelineInfo();

    internal void AddFiles(double currentTime, int startlayer, IEnumerable<File> files)
    {
        files = files.OrderBy(a => a.FullName);
        foreach (var file in files)
        {
            if (file.Duration == null) continue;

            var start = currentTime;
            currentTime += file.Duration.Value;
            var layer = startlayer;
            foreach (var videoStream in file.VideoStreams)
            {
                VideoClips.Add(
                    new TimelineClipVideo(this, videoStream)
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
                    new TimelineClipAudio(this, audioStream)
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
