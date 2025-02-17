using VideoEditor.Enums;

namespace VideoEditor.Forms;

public struct LayerMediaFormat
{
    public LayerMediaFormat(MediaFormat video, int layerIndex)
    {
        MediaFormat = video;
        Layer = layerIndex;
    }

    public MediaFormat MediaFormat { get; }
    public int Layer { get; }
}