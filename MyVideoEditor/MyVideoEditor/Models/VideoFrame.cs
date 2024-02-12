
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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

        public static VideoFrame FromImage(Bitmap image)
        {
            // Lock the bitmap data for direct access
            var bitmap = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            // Get a pointer to the pixel data
            var ptr = bitmap.Scan0;
            var size = image.Width * image.Height * 3;
            var framedata = new byte[size];

            for (int y = 0; y < image.Height; y++)
                for (int x = 0; x < image.Width; x++)
                {
                    var imgoffset = bitmap.Stride * y + x * 4;
                    var red = Marshal.ReadByte(ptr, imgoffset + 0);
                    var green = Marshal.ReadByte(ptr, imgoffset + 1);
                    var blue = Marshal.ReadByte(ptr, imgoffset + 2);
                    var alpha = Marshal.ReadByte(ptr, imgoffset + 3);

                    var frameoffset = image.Width * y * 3 + x * 3;
                    framedata[frameoffset + 0] = blue;
                    framedata[frameoffset + 1] = green;
                    framedata[frameoffset + 2] = red;
                }

            return new VideoFrame(framedata, image.Width, image.Height, 0);
        }

        public Color GetPixel(int x, int y)
        {
            var frameoffset = Width * y * 3 + x * 3;
            return Color.FromArgb(
                FrameData[frameoffset + 0],
                FrameData[frameoffset + 1],
                FrameData[frameoffset + 2]);

        }
        public void SetPixel(int x, int y, Color difpixel)
        {
            var pos = (y * Width + x) * 3;
            FrameData[pos + 0] = difpixel.R;
            FrameData[pos + 1] = difpixel.G;
            FrameData[pos + 2] = difpixel.B;
        }

        public VideoFrame Clone()
        {
            var bytes = new byte[FrameData.Length];
            Array.Copy(FrameData, bytes, bytes.Length);
            return new VideoFrame(bytes, Width, Height, Index);
        }

    }
}