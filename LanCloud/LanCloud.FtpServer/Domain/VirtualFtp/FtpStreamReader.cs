using LanCloud.Domain.Application;
using LanCloud.Domain.IO;
using LanCloud.Models;
using LanCloud.Shared.Log;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace LanCloud.Domain.VirtualFtp
{
    public class FtpStreamReader : Stream
    {
        public FtpStreamReader(PathInfo pathInfo, ILogger logger)
        {
            PathInfo = pathInfo;
            Logger = logger;

            FileBitReaders = FileRef.FileRefBits
                .Select(fileRefBit =>
                {
                    var fileBit = Application.LocalShares
                        .FindFileBits(PathInfo.Extention, FileRef, fileRefBit)
                        .FirstOrDefault();
                    return new FileBitReader(this, fileBit, Logger);
                })
                .OrderBy(a => a.Indexes.Length)
                .ThenBy(a => a.Indexes.OrderBy(b => b).First())
                .ToArray();
            AllIndexes = FileBitReaders
                .SelectMany(a => a.Indexes)
                .GroupBy(a => a)
                .Select(a => a.Key)
                .OrderBy(a => a)
                .ToArray();
            Buffer = new DoubleBuffer(AllIndexes.Length);

            Thread = new Thread(new ThreadStart(Start));
            Thread.Start();
        }

        public PathInfo PathInfo { get; }
        public ILogger Logger { get; }
        internal FileBitReader[] FileBitReaders { get; }
        public int[] AllIndexes { get; }
        public DoubleBuffer Buffer { get; }
        public Thread Thread { get; }
        public bool Disposed { get; private set; }
        public override long Position { get; set; }
        private bool BufferInitialized { get; set; }
        private int BufferPosition { get; set; }

        private AutoResetEvent StartNext { get; } = new AutoResetEvent(true);
        private AutoResetEvent BufferIsWritten { get; } = new AutoResetEvent(false);

        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => false;
        public override long Length => PathInfo.Length.Value;
        public bool EndOfFile => FileBitReaders.All(a => a.Buffer.ReadBufferPosition <= 0);

        public LocalApplication Application => PathInfo.Application;
        public FileRef FileRef => PathInfo.FileRef;
        bool KillSwitch { get; set; }

        private void Start()
        {
            while (!KillSwitch)
            {
                if (StartNext.WaitOne(100))
                {
                    Buffer.WriteBufferPosition = 0;

                    foreach (var item in FileBitReaders)
                    {
                        item.FlipBuffer();

                        if (item.Indexes.Length == 1 && item.Buffer.ReadBufferPosition > 0)
                        {
                            var buffer = item.Buffer.ReadBuffer;
                            var read = item.Buffer.ReadBufferPosition;

                            Array.Copy(buffer, 0, Buffer.WriteBuffer, Buffer.WriteBufferPosition, read);
                            Buffer.WriteBufferPosition += read;
                        }
                    }

                    BufferIsWritten.Set();
                }
            }
        }

        private void FlipBuffer()
        {
            BufferIsWritten.WaitOne();
            Buffer.Flip();
            StartNext.Set();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!BufferInitialized)
            {
                FlipBuffer();
                BufferInitialized = true;
            }

            var read = 0;
            while (read < count && BufferPosition < Buffer.ReadBufferPosition)
            {
                var availableSpace = Buffer.ReadBufferPosition - BufferPosition;
                int bytesToWrite = Math.Min(count - read, availableSpace);

                Array.Copy(Buffer.ReadBuffer, BufferPosition, buffer, offset + read, bytesToWrite);

                read += bytesToWrite;
                BufferPosition += bytesToWrite;
                Position += bytesToWrite;

                if (BufferPosition == Buffer.ReadBuffer.Length)
                {
                    FlipBuffer();
                    BufferPosition = 0;
                }
            }
            return read;
        }

        protected override void Dispose(bool disposing)
        {
            if (!Disposed && disposing)
            {
                KillSwitch = true;
                Disposed = true;

                foreach (var part in FileBitReaders)
                {
                    part.Dispose();
                }
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
