namespace LanCloud.Domain.IO
{
    public class FileRef
    {
        public FileRef()
        {

        }

        public FileRef(FtpFileInfo virtualFileInfo)
        {
            Path = virtualFileInfo?.Path;
            Name = virtualFileInfo?.Name;
            Extention = virtualFileInfo?.Extention;
            Length = virtualFileInfo.FileRef?.Length;
            Hash = virtualFileInfo.FileRef?.Hash;
            FileRefBits = virtualFileInfo.FileRef?.FileRefBits;
        }

        public string Path { get; set; }
        public string Name { get; set; }
        public string Extention { get; set; }
        public long? Length { get; set; }
        public string Hash { get; set; }
        public FileRefBit[] FileRefBits { get; set; }
    }
}