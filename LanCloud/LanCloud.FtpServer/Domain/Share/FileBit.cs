using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;

namespace LanCloud.Domain.Share
{
    public class FileBit
    {
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