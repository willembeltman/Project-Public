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
            ShareParts = FtpFileInfo.Application.Shares
                .SelectMany(share => share.ShareParts)
                .Select(sharepart => new FtpStreamWriterSharePart(this, sharepart))
                .ToArray();
            AllIndexes = ShareParts
                .SelectMany(a => a.Indexes)
                .GroupBy(a => a)
                .Select(a => a.Key)
                .OrderBy(a => a)
                .ToArray();
            Buffer = new DoubleBuffer(AllIndexes.Length);
        }

        public FtpFileInfo FtpFileInfo { get; }
        public FtpStreamWriterSharePart[] ShareParts { get; }
        public int[] AllIndexes { get; }
        public DoubleBuffer Buffer { get; }
        private IncrementalHash IncrementalHash { get; } = IncrementalHash.CreateHash(HashAlgorithmName.MD5);
        public string GeneratedHash { get; private set; }
        public bool Disposed { get; private set; }
        public override long Position { get; set; }

        public override bool CanRead => false;
        public override bool CanSeek => false;
        public override bool CanWrite => true;

        private void FlipBuffer()
        {
            WaitForDone();

            Buffer.Flip();

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
                var availableSpace = Buffer.WriteBuffer.Length - Buffer.WriteBufferWritten;
                int bytesToWrite = Math.Min(count - bytesWritten, availableSpace);

                Array.Copy(buffer, offset + bytesWritten, Buffer.WriteBuffer, Buffer.WriteBufferWritten, bytesToWrite);
                IncrementalHash.AppendData(buffer, offset + bytesWritten, bytesToWrite);

                bytesWritten += bytesToWrite;
                Buffer.WriteBufferWritten += bytesToWrite;
                Position += bytesWritten;

                if (Buffer.WriteBufferWritten >= Buffer.WriteBuffer.Length)
                {
                    FlipBuffer();
                }
            }
        }
        public override void Flush()
        {
            if (Buffer.WriteBufferWritten > 0)
            {
                FlipBuffer();
            }
        }

        // Override de Dispose methode
        protected override void Dispose(bool disposing)
        {
            if (!Disposed && disposing)
            {
                Disposed = true;

                if (Buffer.WriteBufferWritten > 0)
                {
                    FlipBuffer();
                }
                WaitForDone();

                var hashBytes = IncrementalHash.GetHashAndReset();
                var hashString = new StringBuilder(hashBytes.Length * 2);
                foreach (byte b in hashBytes)
                    hashString.Append(b.ToString("x2"));
                GeneratedHash = hashString.ToString();

                var fileBits = ShareParts
                    .Select(a => a.Stop())
                    .ToArray();

                // Waardes updaten
                var fileRef = new FileRef(FtpFileInfo);
                fileRef.Length = Position;
                fileRef.Hash = GeneratedHash;
                fileRef.FileRefBits = fileBits
                    .Select(a => new FileRefBit()
                    {
                        HostName = FtpFileInfo.Application.ServerConfig.HostName,
                        Parts = a.Parts
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
