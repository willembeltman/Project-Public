using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanCloud.Domain.IO
{
    public class DoubleBuffer
    {
        public DoubleBuffer(int width)
        {
            Width = width;
            Buffer1 = new byte[width * Constants.BufferSize];
            Array.Clear(Buffer1, 0, Buffer1.Length);
            BufferWritten1 = 0;
            Buffer2 = new byte[width * Constants.BufferSize];
            Array.Clear(Buffer2, 0, Buffer2.Length);
            BufferWritten2 = 0;
        }

        public int Width { get; }
        private byte[] Buffer1 { get; }
        private byte[] Buffer2 { get; }
        private int BufferWritten1 { get; set; }
        private int BufferWritten2 { get; set; }
        public bool CurrentBufferSwitch { get; set; }

        public byte[] WriteBuffer => CurrentBufferSwitch ? Buffer1 : Buffer2;
        public int WriteBufferWritten
        {
            get => CurrentBufferSwitch ? BufferWritten1 : BufferWritten2;
            set
            {
                if (CurrentBufferSwitch)
                    BufferWritten1 = value;
                else
                    BufferWritten2 = value;
            }
        }

        public byte[] ReadBuffer => CurrentBufferSwitch ? Buffer2 : Buffer1;
        public int ReadBufferWritten => CurrentBufferSwitch ? BufferWritten2 : BufferWritten1;

        internal void Flip()
        {
            CurrentBufferSwitch = !CurrentBufferSwitch;
            WriteBufferWritten = 0;
        }
    }
}
