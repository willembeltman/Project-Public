using Newtonsoft.Json;
using System.IO;

namespace LanCloud.Domain.Application
{
    public class FileRef
    {
        public string VirtualFullName { get; set; }
        public string Extention { get; }
        public long Size { get; }
        public string Hash { get; }
        public FileRefBit[] FileRefBits { get; set; }

        public static FileRef Load(FileInfo fileInfo)
        {
            using (var stream = fileInfo.OpenRead())
            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<FileRef>(json);
            }
        }
    }
}