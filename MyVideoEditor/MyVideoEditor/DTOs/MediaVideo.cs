using MyVideoEditor.Models;

namespace MyVideoEditor.DTOs
{
    public class MediaVideo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid MediaId { get; set; }
        public int StreamIndex { get; set; }
        public double? Duration { get; set; }
        public Size? Resolution { get; set; }
        public long? FramerateBase { get; set; }
        public long? FramerateDivider { get; set; }
    }
}