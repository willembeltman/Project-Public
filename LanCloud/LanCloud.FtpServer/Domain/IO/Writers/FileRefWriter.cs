using LanCloud.Domain.Application;
using LanCloud.Domain.VirtualFtp;
using LanCloud.Shared.Log;
using System;
using System.IO;
using System.Linq;

namespace LanCloud.Domain.IO.Writers
{
    public class FileRefWriter : Stream
    {
        public FileRefWriter(PathFileInfo pathInfo, ILogger logger)
        {
            PathInfo = pathInfo;
            Logger = logger;

            HashWriter = new HashBuffer(this, Logger);
            DataBitWriters = Application.LocalShareParts
                .Where(a => a.Indexes.Length == 1)
                .GroupBy(a => a.Indexes.First())
                .Select(sharepart => new DataBitBuffer(this, sharepart.Key, sharepart.ToArray(), Logger))
                .ToArray();
            ParityBitWriters = Application.LocalShareParts
                .Where(a => a.Indexes.Length > 1)
                .GroupBy(a => string.Join("_", a.Indexes.OrderBy(b => b)))
                .Select(sharepart => new ParityBitBuffer(this, sharepart.ToArray(), Logger))
                .ToArray();

            AllIndexes = Application.LocalShareParts
                .SelectMany(a => a.Indexes)
                .GroupBy(a => a)
                .Select(a => a.Key)
                .OrderBy(a => a)
                .ToArray();
            Buffer = new DoubleBuffer(AllIndexes.Length);

            Logger.Info($"Opened virtual ftp file: {pathInfo.Name}");
        }

        public PathFileInfo PathInfo { get; }
        public ILogger Logger { get; }
        public DataBitBuffer[] DataBitWriters { get; }
        public ParityBitBuffer[] ParityBitWriters { get; }
        public HashBuffer HashWriter { get; }
        public byte[] AllIndexes { get; }
        public DoubleBuffer Buffer { get; }
        public bool Disposed { get; private set; }
        public override long Position { get; set; }

        public override bool CanRead => false;
        public override bool CanSeek => false;
        public override bool CanWrite => true;

        public LocalApplication Application => PathInfo.Application;

        public override void Write(byte[] buffer, int offset, int count)
        {
            var bytesWritten = 0;

            while (bytesWritten < count)
            {
                var availableSpace = Buffer.WriteBuffer.Length - Buffer.WriteBufferPosition;
                int bytesToWrite = Math.Min(count - bytesWritten, availableSpace);

                Array.Copy(buffer, offset + bytesWritten, Buffer.WriteBuffer, Buffer.WriteBufferPosition, bytesToWrite);

                bytesWritten += bytesToWrite;
                Buffer.WriteBufferPosition += bytesToWrite;
                Position += bytesWritten;

                if (Buffer.WriteBufferPosition >= Buffer.WriteBuffer.Length)
                {
                    FlipBuffer();
                    Buffer.WriteBufferPosition = 0;
                }
            }
        }
        public override void Flush()
        {
            if (Buffer.WriteBufferPosition > 0)
            {
                FlipBuffer();
            }
        }
        private void FlipBuffer()
        {
            WaitForDone();

            Buffer.Flip();

            HashWriter.StartNext.Set();

            foreach (var item in DataBitWriters)
                item.StartNext.Set();

            foreach (var item in ParityBitWriters)
                item.StartNext.Set();
        }

        private void WaitForDone()
        {
            if (!HashWriter.WritingIsDone.WaitOne(100000))
                throw new Exception("Timeout writing to HashWriter");

            foreach (var item in DataBitWriters)
                if (!item.WritingIsDone.WaitOne(100000))
                    throw new Exception("Timeout writing to DataBitWriters");

            foreach (var item in ParityBitWriters)
                if (!item.WritingIsDone.WaitOne(100000))
                    throw new Exception("Timeout writing to ParityBitWriters");
        }

        protected override void Dispose(bool disposing)
        {
            if (!Disposed && disposing)
            {
                Disposed = true;

                if (Buffer.WriteBufferPosition > 0)
                {
                    FlipBuffer();
                }
                WaitForDone();

                var hash = HashWriter.Stop();
                var fileBits = DataBitWriters
                    .SelectMany(a => a.Stop(Position, hash))
                    .ToArray();
                var fileBits2 = ParityBitWriters
                    .SelectMany(a => a.Stop(Position, hash))
                    .ToArray();

                // Waardes updaten
                var fileRef = new FileRef(PathInfo);
                fileRef.Length = Position;
                fileRef.Hash = hash;
                fileRef.Bits = fileBits
                    .Concat(fileBits2)
                    .Select(a => new FileRefBit()
                    {
                        Indexes = a.Indexes
                    })
                    .ToArray();
                PathInfo.FileRef = fileRef;
            }

            base.Dispose(disposing);
        }

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
