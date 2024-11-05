using Newtonsoft.Json;

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
            Length = virtualFileInfo.FileRef?.Length;
            Hash = virtualFileInfo.FileRef?.Hash;
            FileRefBits = virtualFileInfo.FileRef?.FileRefBits;
        }

        public string Path { get; set; }
        [JsonIgnore]
        public string Name => FtpPathTranslator.TranslatePathToName(Path);
        [JsonIgnore]
        public string Extention => FtpPathTranslator.TranslatePathToExtention(Path);
        public long? Length { get; set; }
        public string Hash { get; set; }
        public FileRefBit[] FileRefBits { get; set; }
    }
}