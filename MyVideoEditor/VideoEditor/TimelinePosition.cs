using VideoEditor.Enums;

namespace VideoEditor;

public struct TimelinePosition
{
    public TimelinePosition(MediaFormat mediaFormat, double currentTime, int layerIndex)
    {
        MediaFormat = mediaFormat;
        CurrentTime = currentTime;
        Layer = layerIndex;
    }

    public MediaFormat MediaFormat { get; }
    public double CurrentTime { get; }
    public int Layer { get; }
}
