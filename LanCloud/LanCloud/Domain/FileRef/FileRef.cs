using LanCloud.Domain.VirtualFtp;

namespace LanCloud.Models
{
    public class FileRef
    {
        public FileRef(long? length, string hash, FileRefStripe[] bits)
        {
            Length = length;
            Hash = hash;
            Bits = bits;
        }

        public FileRef(FileRefInfo pathInfo)
        {
            Length = pathInfo.FileRef?.Length;
            Hash = pathInfo.FileRef?.Hash;
            Bits = pathInfo.FileRef?.Bits;
        }

        public long? Length { get; }
        public string Hash { get; }
        public FileRefStripe[] Bits { get; }
    }
}