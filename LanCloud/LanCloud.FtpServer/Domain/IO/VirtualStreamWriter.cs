using LanCloud.Domain.Application;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace LanCloud.Domain.IO
{
    public class VirtualStreamWriter : Stream
    {
        public VirtualStreamWriter(VirtualFileInfo virtualFileInfo)
        {
            VirtualFileInfo = virtualFileInfo;
            ShareParts = virtualFileInfo.Application.Shares
                .SelectMany(share => share.ShareParts)
                .Select(sharepart => new VirtualStreamWriterSharePart(this, sharepart))
                .ToArray();
            Indexes = ShareParts
                .SelectMany(a => a.Part.Indexes)
                .GroupBy(a => a)
                .Select(a => a.Key)
                .OrderBy(a => a)
                .ToArray();
            Buffer1 = new byte[Width * Constants.BufferSize];
            Buffer2 = new byte[Width * Constants.BufferSize];
        }
        public VirtualFileInfo VirtualFileInfo { get; }
        public VirtualStreamWriterSharePart[] ShareParts { get; }
        public int[] Indexes { get; }
        public int Width => Indexes.Length;
        public byte[] Buffer1 { get; }
        public byte[] Buffer2 { get; }
        public int BufferWritten1 { get; set; }
        public int BufferWritten2 { get; set; }
        public bool CurrentBufferSwitch { get; private set; }
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

        public override long Position { get; set; }

        public void FlipBuffer()
        {
            WaitForDone();

            CurrentBufferSwitch = !CurrentBufferSwitch;
            WriteBufferWritten = 0;

            foreach (var item in ShareParts)
                item.StartNext.Set();
        }

        private void WaitForDone()
        {
            foreach (var item in ShareParts)
                item.ReadingIsDone.WaitOne();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            var bytesWritten = 0;

            while (bytesWritten < count)
            {
                var availableSpace = WriteBuffer.Length - WriteBufferWritten;
                int bytesToWrite = Math.Min(count - bytesWritten, availableSpace);

                Array.Copy(buffer, offset + bytesWritten, WriteBuffer, WriteBufferWritten, bytesToWrite);
                WriteBufferWritten += bytesToWrite;
                bytesWritten += bytesToWrite;

                if (WriteBufferWritten >= WriteBuffer.Length)
                {
                    FlipBuffer();
                }
            }
        }

        public override void Flush()
        {
            if (WriteBufferWritten > 0)
            {
                FlipBuffer();
            }
        }
        public new void Dispose()
        {
            if (WriteBufferWritten > 0)
            {
                FlipBuffer();
            }
            WaitForDone();

            foreach (var conn in ShareParts)
                conn.Dispose();
        }
        public override bool CanRead => false;
        public override bool CanSeek => false;
        public override bool CanWrite => true;

        #region Not implemented

        public override long Length => throw new NotImplementedException();

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
