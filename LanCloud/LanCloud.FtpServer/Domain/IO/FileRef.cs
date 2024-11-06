using LanCloud.Domain.VirtualFtp;
using Newtonsoft.Json;

namespace LanCloud.Domain.IO
{
    public class FileRef
    {
        public FileRef()
        {
        }

        public FileRef(PathFileInfo pathInfo)
        {
            //Path = pathInfo?.Path;
            //Extention = pathInfo.Extention;
            Length = pathInfo.FileRef?.Length;
            Hash = pathInfo.FileRef?.Hash;
            FileRefBits = pathInfo.FileRef?.FileRefBits;
        }

        //[JsonIgnore]
        //public string Path { get; set; }
        //[JsonIgnore]
        //public string Name => PathTranslator.TranslatePathToName(Path);
        //[JsonIgnore]
        //public string Extention => PathTranslator.TranslatePathToExtention(Path);

        //public string Extention { get; set; }
        public long? Length { get; set; }
        public string Hash { get; set; }
        public FileRefBit[] FileRefBits { get; set; }
    }
}