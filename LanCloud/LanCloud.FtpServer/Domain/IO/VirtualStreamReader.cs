using System;
using System.IO;

namespace LanCloud.Domain.IO
{
    public class VirtualStreamReader : Stream
    {
        public VirtualStreamReader(VirtualFileInfo virtualFileInfo)
        {
            VirtualFileInfo = virtualFileInfo;
        }
        public VirtualFileInfo VirtualFileInfo { get; }

        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => false;

        public override long Length => VirtualFileInfo.Length;
        public override long Position { get; set; }


        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }
        public new void Dispose()
        {
            throw new NotImplementedException();
        }

        #region Not implemented

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
