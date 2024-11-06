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
            Bits = pathInfo.FileRef?.Bits;
        }

        //[JsonIgnore]
        //public string Path { get; set; }
        //[JsonIgnore]
        //public string Name => PathTranslator.TranslatePathToName(Path);
        //[JsonIgnore]
        //public string Extention => PathTranslator.TranslatePathToExtention(Path);

        //public string Extention { get; set; }
        [JsonProperty("l")]
        public long? Length { get; set; }
        [JsonProperty("h")]
        public string Hash { get; set; }
        [JsonProperty("b")]
        public FileRefBit[] Bits { get; set; }
    }
}