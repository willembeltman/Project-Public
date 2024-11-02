using Newtonsoft.Json;
using System;
using System.IO;

namespace LanCloud.Models
{
    public class FileBit
    {
        public FileBit(FileInfo jsonFile)
        {
            JsonFile = jsonFile;

            var dataFullName = CreateDataFullName(jsonFile);
            DataFile = new FileInfo(dataFullName);

            if (!DataFile.Exists) throw new Exception("Error reading file bit data");

            var json = File.ReadAllText(jsonFile.FullName);
            Information = JsonConvert.DeserializeObject<FileBitInformation>(json);
        }

        public FileBit(FileInfo jsonFile, FileBitInformation information, byte[] data)
        {
            JsonFile = jsonFile;

            var dataFullName = CreateDataFullName(JsonFile);
            DataFile = new FileInfo(dataFullName);
            using (var fileStream = DataFile.OpenWrite())
            using (var streamWriter = new StreamWriter(fileStream))
            {
                streamWriter.Write(data);
            }

            Information = information;
            var json = JsonConvert.SerializeObject(information);
            File.WriteAllText(JsonFile.FullName, json);
        }

        private static string CreateDataFullName(FileInfo jsonFile)
        {
            var dataNameLength = jsonFile.Name.Length - jsonFile.Extension.Length;
            var dataName = jsonFile.Name.Substring(0, dataNameLength) + ".data";
            var dataFullname = Path.Combine(jsonFile.Directory.FullName, dataName);
            return dataFullname;
        }


        public FileInfo JsonFile { get; }
        private FileInfo DataFile { get; }
        public FileBitInformation Information { get; }

        public Stream GetDataStream()
        {
            return DataFile.OpenRead();
        }

        internal void SaveInformation()
        {
            throw new NotImplementedException();
        }
    }
}