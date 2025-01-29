using VideoEditor.Types;

namespace VideoEditor;

public class TimelineInfo
{
    public TimelineInfo(Resolution? resolution = null, Fps? fps = null, int sampleRate = 48000, int channels = 2)
    {
        Resolution = resolution ?? new Resolution(1920, 1080);
        Fps = fps ?? new Fps(25, 1);
        SampleRate = sampleRate;
        Channels = channels;
    }
    public Resolution Resolution { get; set; }
    public Fps Fps { get; set; }
    public int SampleRate { get; set; }
    public int Channels { get; set; }
}

