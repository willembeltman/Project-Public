using LanCloud.Domain.Application;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LanCloud.Domain.IO
{
    public class VirtualStreamWriter : Stream
    {

        public VirtualStreamWriter(VirtualFileInfo virtualFileInfo)
        {
            VirtualFileInfo = virtualFileInfo;
            ShareParts = Application.Shares
                .SelectMany(share => share.ShareParts)
                .Select(sharepart => new VirtualStreamWriterSharePart(this, sharepart))
                .ToArray();
            AllIndexes = ShareParts
                .SelectMany(a => a.Part.Indexes)
                .GroupBy(a => a)
                .Select(a => a.Key)
                .OrderBy(a => a)
                .ToArray();
            Buffer1 = new byte[Width * Constants.BufferSize];
            Array.Clear(Buffer1, 0, Buffer1.Length);
            BufferWritten1 = 0;
            Buffer2 = new byte[Width * Constants.BufferSize];
            Array.Clear(Buffer2, 0, Buffer2.Length);
            BufferWritten2 = 0;
        }
        public VirtualFileInfo VirtualFileInfo { get; }
        public VirtualStreamWriterSharePart[] ShareParts { get; }
        public int[] AllIndexes { get; }

        private byte[] Buffer1 { get; }
        private byte[] Buffer2 { get; }
        private int BufferWritten1 { get; set; }
        private int BufferWritten2 { get; set; }
        private bool CurrentBufferSwitch { get; set; }
        private IncrementalHash IncrementalHash { get; } = IncrementalHash.CreateHash(HashAlgorithmName.MD5);

        public string Hash { get; private set; }
        public bool Disposed { get; private set; }

        public LocalApplication Application => VirtualFileInfo.Application;

        public int Width => AllIndexes.Length;
        private byte[] WriteBuffer => CurrentBufferSwitch ? Buffer1 : Buffer2;
        private int WriteBufferWritten
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
                IncrementalHash.AppendData(buffer, offset + bytesWritten, bytesToWrite);

                bytesWritten += bytesToWrite;
                WriteBufferWritten += bytesToWrite;
                Position += bytesWritten;

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

        // Override de Dispose methode
        protected override void Dispose(bool disposing)
        {
            if (!Disposed && disposing)
            {
                Disposed = true;

                if (WriteBufferWritten > 0)
                {
                    FlipBuffer();
                }
                WaitForDone();

                var hashBytes = IncrementalHash.GetHashAndReset();
                var hashString = new StringBuilder(hashBytes.Length * 2);
                foreach (byte b in hashBytes)
                    hashString.Append(b.ToString("x2"));
                Hash = hashString.ToString();

                var fileBits = ShareParts
                    .Select(a => a.Stop())
                    .ToArray();

                // Waardes updaten
                var fileRef = new FileRef(VirtualFileInfo);
                fileRef.Length = Position;
                fileRef.Hash = Hash;
                fileRef.FileRefBits = fileBits
                    .Select(a => new FileRefBit()
                    {
                        HostName = Application.ServerConfig.HostName,
                        Parts = a.Parts
                    })
                    .ToArray();
                VirtualFileInfo.FileRef = fileRef;
            }

            base.Dispose(disposing);
        }
        public new void Dispose()
        {
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
