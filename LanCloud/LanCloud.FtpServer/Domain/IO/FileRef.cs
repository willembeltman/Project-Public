using LanCloud.Domain.VirtualFtp;

namespace LanCloud.Domain.IO
{
    public class FileRef
    {
        public FileRef(long? length, string hash, FileRefBit[] bits)
        {
            Length = length;
            Hash = hash;
            Bits = bits;
        }

        public FileRef(PathFileInfo pathInfo)
        {
            Length = pathInfo.FileRef?.Length;
            Hash = pathInfo.FileRef?.Hash;
            Bits = pathInfo.FileRef?.Bits;
        }

        public long? Length { get; }
        public string Hash { get; }
        public FileRefBit[] Bits { get; }
    }
}