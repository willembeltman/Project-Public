using LanCloud.Domain.VirtualFtp;
using LanCloud.Shared.Log;
using System;
using System.IO;

namespace LanCloud.Domain.IO.Readers
{
    public class FileRefReader : Stream
    {
        public FileRefReader(PathFileInfo pathInfo, ILogger logger)
        {
            PathInfo = pathInfo;
            Logger = logger;

            FileBitJoiner = new FileBitsBuffer(this, logger);
        }

        public PathFileInfo PathInfo { get; }
        public ILogger Logger { get; }
        public FileBitsBuffer FileBitJoiner { get; }

        public override long Position { get; set; }
        private bool BufferInitialized { get; set; }
        private int BufferPosition { get; set; }

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => PathInfo.Length.Value;
        public bool Disposed => FileBitJoiner.Disposed;

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!BufferInitialized)
            {
                FileBitJoiner.FlipBuffer();
                BufferInitialized = true;
            }

            var read = 0;
            while (read < count && BufferPosition < FileBitJoiner.Buffer.ReadBufferPosition)
            {
                var availableSpace = FileBitJoiner.Buffer.ReadBufferPosition - BufferPosition;
                int bytesToWrite = Math.Min(count - read, availableSpace);

                Array.Copy(FileBitJoiner.Buffer.ReadBuffer, BufferPosition, buffer, offset + read, bytesToWrite);

                read += bytesToWrite;
                BufferPosition += bytesToWrite;
                Position += bytesToWrite;

                if (BufferPosition == FileBitJoiner.Buffer.ReadBuffer.Length)
                {
                    FileBitJoiner.FlipBuffer();
                    BufferPosition = 0;
                }
            }
            return read;
        }

        protected override void Dispose(bool disposing)
        {
            if (!Disposed && disposing)
            {
                FileBitJoiner.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Not implemented

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
        public override void Flush()
        {
            throw new NotImplementedException();
        }
        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
