using LanCloud.Domain.Application;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LanCloud.Domain.IO
{
    public class FtpStreamWriter : Stream
    {
        public FtpStreamWriter(FtpFileInfo ftpFileInfo)
        {
            FtpFileInfo = ftpFileInfo;
            FileBitWriters = Application.LocalShares
                .SelectMany(share => share.Parts)
                .Select(sharepart => new FileBitWriter(this, sharepart))
                .ToArray();
            AllIndexes = FileBitWriters
                .SelectMany(a => a.Indexes)
                .GroupBy(a => a)
                .Select(a => a.Key)
                .OrderBy(a => a)
                .ToArray();
            Buffer = new DoubleBuffer(AllIndexes.Length);
        }

        public FtpFileInfo FtpFileInfo { get; }
        public FileBitWriter[] FileBitWriters { get; }
        public int[] AllIndexes { get; }
        public DoubleBuffer Buffer { get; }
        private IncrementalHash IncrementalHash { get; } = IncrementalHash.CreateHash(HashAlgorithmName.MD5);
        public string GeneratedHash { get; private set; }
        public bool Disposed { get; private set; }
        public override long Position { get; set; }

        public override bool CanRead => false;
        public override bool CanSeek => false;
        public override bool CanWrite => true;

        public LocalApplication Application => FtpFileInfo.Application;

        private void FlipBuffer()
        {
            WaitForDone();

            Buffer.Flip();

            foreach (var item in FileBitWriters)
                item.StartNext.Set();
        }

        private void WaitForDone()
        {
            foreach (var item in FileBitWriters)
                if (!item.ReadingIsDone.WaitOne(10000))
                    throw new Exception("Timeout writing to: " + item.FileBit?.FullName);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            var bytesWritten = 0;

            while (bytesWritten < count)
            {
                var availableSpace = Buffer.WriteBuffer.Length - Buffer.WriteBufferPosition;
                int bytesToWrite = Math.Min(count - bytesWritten, availableSpace);

                Array.Copy(buffer, offset + bytesWritten, Buffer.WriteBuffer, Buffer.WriteBufferPosition, bytesToWrite);
                IncrementalHash.AppendData(buffer, offset + bytesWritten, bytesToWrite);

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

                var hashBytes = IncrementalHash.GetHashAndReset();
                var hashString = new StringBuilder(hashBytes.Length * 2);
                foreach (byte b in hashBytes)
                    hashString.Append(b.ToString("x2"));
                GeneratedHash = hashString.ToString();

                var fileBits = FileBitWriters
                    .Select(a => a.Stop())
                    .ToArray();

                // Waardes updaten
                var fileRef = new FileRef(FtpFileInfo);
                fileRef.Length = Position;
                fileRef.Hash = GeneratedHash;
                fileRef.FileRefBits = fileBits
                    .Select(a => new FileRefBit()
                    {
                        Indexes = a.Indexes,
                        Length = a.Info.Length
                    })
                    .ToArray();
                FtpFileInfo.FileRef = fileRef;
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
