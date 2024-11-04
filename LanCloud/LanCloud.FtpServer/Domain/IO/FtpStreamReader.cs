using System;
using System.IO;
using System.Linq;

namespace LanCloud.Domain.IO
{
    public class FtpStreamReader : Stream
    {
        public FtpStreamReader(FtpFileInfo ftpFileInfo)
        {
            FtpFileInfo = ftpFileInfo;
            ShareParts = FtpFileInfo.FileRef.FileRefBits
                .Select(fileRefBit => new FtpStreamReaderFileRefBit(this, fileRefBit))
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
        internal FtpStreamReaderFileRefBit[] ShareParts { get; }
        public int[] AllIndexes { get; }
        public DoubleBuffer Buffer { get; }
        public bool Disposed { get; private set; }
        public override long Position { get; set; }

        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => false;
        public override long Length => FtpFileInfo.Length.Value;

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

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
        // Override de Dispose methode
        protected override void Dispose(bool disposing)
        {
            if (!Disposed && disposing)
            {
                Disposed = true;

                //todo
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
