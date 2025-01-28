namespace VideoEditor.Info;

public class AudioInfo
{
    public AudioInfo(int sampleRate = 48000, int channels = 2)
    {
        SampleRate = sampleRate;
        Channels = channels;
    }
    public int SampleRate { get; set; }
    public int Channels { get; set; }
}