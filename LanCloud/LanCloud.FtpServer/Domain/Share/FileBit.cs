using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;

namespace LanCloud.Domain.Share
{
    public class FileBit
    {
        //public FileBit(FileInfo fileRefInfo)
        //{
        //    FileRefFileInfo = fileRefInfo;
        //    VirtualPath = FileRefFileInfo.Directory.FullName.Substring(Root.FullName.Length);

        //    var dataFullName = CreateDataFullName(fileRefInfo);
        //    DataFileInfo = new FileInfo(dataFullName);

        //    if (!DataFileInfo.Exists) throw new Exception("Error reading file bit data");

        //    var json = File.ReadAllText(fileRefInfo.FullName);
        //    Info = JsonConvert.DeserializeObject<FileRefInfo>(json);
        //}

        //public FileBit(FileInfo jsonFile, FileRefInfo info, byte[] data)
        //{
        //    FileRefFileInfo = jsonFile;
        //    VirtualPath = FileRefFileInfo.Directory.FullName.Substring(Root.FullName.Length);

        //    var dataFullName = CreateDataFullName(FileRefFileInfo);
        //    DataFileInfo = new FileInfo(dataFullName);
        //    using (var fileStream = DataFileInfo.OpenWrite())
        //    using (var streamWriter = new StreamWriter(fileStream))
        //    {
        //        streamWriter.Write(data);
        //    }

        //    Info = info;
        //    var json = JsonConvert.SerializeObject(info);
        //    File.WriteAllText(FileRefFileInfo.FullName, json);
        //}


        //private static string CreateDataFullName(FileInfo jsonFile)
        //{
        //    var dataNameLength = jsonFile.Name.Length - jsonFile.Extension.Length;
        //    var dataName = jsonFile.Name.Substring(0, dataNameLength) + ".data";
        //    var dataFullname = Path.Combine(jsonFile.Directory.FullName, dataName);
        //    return dataFullname;
        //}

        //public FileRefInfo Info { get; }
        //public string VirtualPath { get; }
        //public FileInfo FileRefFileInfo { get; }
        //public FileInfo DataFileInfo { get; }

        //public Stream GetDataStream()
        //{
        //    return DataFileInfo.OpenRead();
        //}
        public FileBit(FileInfo info)
        {
            Info = info;
            FullName = info.FullName;
            var split = info.Name.Split('.');
            Extention = split[0];
            Size = Convert.ToInt64(split[1]);
            Part = Convert.ToInt32(split[2]);
            Hash = split[3];
        }
        public FileBit(DirectoryInfo dirinfo, string extention, long size, int part, string hash)
        {
            Extention = extention;
            Size = size;
            Part = part;
            Hash = hash;
            var name = $"{Extention}.{Size}.{Part}.{Hash}";
            FullName = Path.Combine(dirinfo.FullName, name);
            Info = new FileInfo(FullName);
        }

        public FileInfo Info { get; }
        public string FullName { get; }
        public string Extention { get; }
        public long Size { get; }
        public int Part { get; }
        public string Hash { get; }

        public bool Exists() { return Info.Exists; }
        public FileBitStreamReader OpenRead()
        {
            return new FileBitStreamReader(Info);
        }
        public FileBitStreamWriter OpenWrite()
        {
            return new FileBitStreamWriter(Info);
        }
    }
}