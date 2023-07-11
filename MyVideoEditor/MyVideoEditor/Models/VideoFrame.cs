namespace MyVideoEditor.Models
{
    public class VideoFrame
    {
        public VideoFrame(byte[] frameData, int width, int height)
        {
            FrameData = frameData;
            Width = width;
            Height = height;
        }

        public byte[] FrameData { get; }
        public int Width { get; }
        public int Height { get; }
    }
}