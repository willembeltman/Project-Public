namespace MyVideoEditor.Models
{
    public class VideoFrame
    {
        public VideoFrame(byte[] frameData, int width, int height, long index)
        {
            FrameData = frameData;
            Width = width;
            Height = height;
            Index = index;
        }

        public long Index { get; }
        public byte[] FrameData { get; }
        public int Width { get; }
        public int Height { get; }
    }
}