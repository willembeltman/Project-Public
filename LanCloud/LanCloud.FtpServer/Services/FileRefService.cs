using LanCloud.Domain.IO;
using Newtonsoft.Json;
using System.IO;

namespace LanCloud.Services
{
    public static class FileRefService
    {
        public static FileRef Load(FileInfo fileInfo)
        {
            if (!fileInfo.Exists) return null;
            using (var stream = fileInfo.OpenRead())
            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<FileRef>(json);
            }
        }

        public static FileRef Save(FileInfo fileInfo, FileRef fileRef)
        {
            if (!fileInfo.Exists) fileInfo.Delete();
            using (var stream = fileInfo.OpenWrite())
            using (var writer = new StreamWriter(stream))
            {
                var json = JsonConvert.SerializeObject(fileRef);
                writer.Write(json);
            }

            return fileRef;
        }
    }
}
