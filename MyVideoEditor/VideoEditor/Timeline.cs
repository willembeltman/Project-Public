using System.Threading.Channels;
using VideoEditor.Types;

namespace VideoEditor;

public class Timeline
{
    public Timeline(Project project)
    {
        Project = project;
        Resolution = new Resolution(1920, 1080);
        Fps = new Fps(25, 1);
        SampleRate = 48000;
        AudioChannels = 2;
    }

    public Project Project { get; }

    public ConcurrentArray<TimelineClipVideo> VideoClips { get; } = new ConcurrentArray<TimelineClipVideo>();
    public ConcurrentArray<TimelineClipAudio> AudioClips { get; } = new ConcurrentArray<TimelineClipAudio>();
    public ConcurrentArray<ITimelineClip> AllClips { get; } = new ConcurrentArray<ITimelineClip>();
    public ConcurrentArray<ITimelineClip> SelectedClips { get; } = new ConcurrentArray<ITimelineClip>();

    //public TimelineInfo Info { get; } = new TimelineInfo();

    public Resolution Resolution { get; set; }
    public Fps Fps { get; set; }
    public int SampleRate { get; set; }
    public int AudioChannels { get; set; }

    public double VisibleWidth { get; set; } = 100;
    public double VisibleStart { get; set; } = 0;
    public double PlayerPosition { get; set; } = 0;

    public int FirstVisibleVideoLayer { get; set; } = 0;
    public int FirstVisibleAudioLayer { get; set; } = 0;
    public int VisibleVideoLayers { get; set; } = 3;
    public int VisibleAudioLayers { get; set; } = 3;


    public void AddFiles(IEnumerable<File> files, double currentTime, int layerIndex)
    {
        files = files.OrderBy(a => a.FullName);
        foreach (var file in files)
        {
            if (file.Duration == null) continue;

            var start = currentTime;
            currentTime += file.Duration.Value;
            var layer = layerIndex;
            foreach (var videoStream in file.VideoStreams)
            {
                var clip = new TimelineClipVideo(this, videoStream)
                {
                    Layer = layer,
                    TimelineStartInSeconds = start,
                    TimelineEndInSeconds = currentTime,
                    ClipStartInSeconds = 0,
                    ClipEndInSeconds = file.Duration.Value
                };
                AllClips.Add(clip);
                VideoClips.Add(clip);
                layer++;
            }
            layer = 0;
            foreach (var audioStream in file.AudioStreams)
            {
                var clip = new TimelineClipAudio(this, audioStream)
                {
                    Layer = layer,
                    TimelineStartInSeconds = start,
                    TimelineEndInSeconds = currentTime,
                    ClipStartInSeconds = 0,
                    ClipEndInSeconds = file.Duration.Value
                };
                AllClips.Add(clip);
                AudioClips.Add(clip);
                layer++;
            }
            Project.Files.Add(file);
        }
    }
}
