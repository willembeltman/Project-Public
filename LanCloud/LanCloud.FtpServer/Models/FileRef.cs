namespace LanCloud.Domain.IO
{
    public class FileRef
    {
        public string FullName { get; set; }
        public string Name { get; set; }
        public string Extention { get; }
        public long Size { get; set; }
        public string Hash { get; set; }
        public long Length { get; set; }
        public FileRefBit[] FileRefBits { get; set; }
    }
}