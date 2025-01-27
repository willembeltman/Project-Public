
namespace VideoEditor
{
    public class StreamInfo
    {
        public int Index { get; set; }
        public string? Codec { get; set; }
        public string? CodecTypeString { get; set; }
        public double? Duration { get; set; }
        public int Bitrate { get; set; }
        public Size Resolution { get; set; } 
        public Fps Fps { get; set; } = new Fps();

        public CodecTypeEnum CodecType => CodecTypeString == "video" ? CodecTypeEnum.Video : CodecTypeEnum.Audio;
    }
}