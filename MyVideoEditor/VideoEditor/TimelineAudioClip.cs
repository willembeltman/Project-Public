using VideoEditor.Info;
namespace VideoEditor;

public class TimelineAudioClip : TimelineClip
{
    public TimelineAudioClip(Timeline timeline, File file, StreamInfo audioStream) : base(timeline, file)
    {
        AudioStream = audioStream;
    }

    public StreamInfo AudioStream { get; }

    public double TimelineStartInSeconds
    {
        get => TimelineStartIndex / Timeline.AudioInfo.SampleRate;
        set => TimelineStartIndex = Convert.ToInt64(value * Timeline.AudioInfo.SampleRate);
    }
    public double TimelineEndInSeconds
    {
        get => TimelineEndIndex / Timeline.AudioInfo.SampleRate;
        set => TimelineEndIndex = Convert.ToInt64(value * Timeline.AudioInfo.SampleRate);
    }
    public double ClipStartInSeconds
    {
        get => ClipStartIndex / AudioStream.SampleRate.Value;
        set => ClipStartIndex = Convert.ToInt64(value * AudioStream.SampleRate);
    }
    public double ClipEndInSeconds
    {
        get => ClipEndIndex / AudioStream.SampleRate.Value;
        set => ClipEndIndex = Convert.ToInt64(value * AudioStream.SampleRate);
    }
}

