using VideoEditor.Enums;
namespace VideoEditor.Info;

public class AudioInfo
{
    public AudioInfo(long sampleRate = 48000, BitDepth bitDepth = BitDepth.s16)
    {
        SampleRate = sampleRate;
        BitDepth = bitDepth;
    }
    public long SampleRate { get; set; }
    public BitDepth BitDepth { get; set; }
}