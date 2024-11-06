using System;

namespace LanCloud.Domain.IO
{
    public class SingleBuffer
    {
        public SingleBuffer(int width = 1)
        {
            Width = width;
            Buffer = new byte[width * Constants.BufferSize];
            Array.Clear(Buffer, 0, Buffer.Length);
            BufferPosition = 0;
        }

        public int Width { get; }
        public byte[] Buffer { get; }
        public int BufferPosition { get; set; }
        public int Length => Buffer.Length;
    }
}
