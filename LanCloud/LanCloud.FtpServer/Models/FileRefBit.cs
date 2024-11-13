namespace LanCloud.Models
{
    public class FileRefBit
    {
        public FileRefBit(byte[] indexes)
        {
            Indexes = indexes;
        }

        public byte[] Indexes { get; }
    }
}
