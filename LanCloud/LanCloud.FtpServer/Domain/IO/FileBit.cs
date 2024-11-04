using System;
using System.Data;
using System.IO;
using System.Linq;

namespace LanCloud.Domain.IO
{
    public class FileBit
    {
        public FileBit(FileInfo info)
        {
            Info = info;
            FullName = info.FullName;
            var split = info.Name.Split('.');
            Extention = split[0];
            Parts = split[1].Split('_').Select(a => Convert.ToInt32(a)).ToArray();
            Size = Convert.ToInt64(split[2]);
            Hash = split[3];
        }
        public FileBit(DirectoryInfo dirinfo, string extention, int[] parts, long size, string hash)
        {
            Extention = extention;
            Size = size;
            Parts = parts;
            Hash = hash;
            var name = $"{extention}.{string.Join("_", parts)}.{size}.{hash}.filebit";
            FullName = Path.Combine(dirinfo.FullName, name);
            Info = new FileInfo(FullName);
        }
        public FileBit(DirectoryInfo dirinfo, string extention, int[] parts)
        {
            Extention = extention;
            Parts = parts;
            var name = $"{extention}.{string.Join("_", parts)}.{Guid.NewGuid().ToString()}.tempbit";
            FullName = Path.Combine(dirinfo.FullName, name);
            Info = new FileInfo(FullName);
            IsTemp = true;
        }

        public string Extention { get; }
        public int[] Parts { get; }
        public FileInfo Info { get; private set; }
        public bool IsTemp { get; private set; }
        public string FullName { get; private set; }
        public long Size { get; private set; }
        public string Hash { get; private set; }

        public void Update(long size, string hash)
        {
            Size = size;
            Hash = hash;
            var name = $"{Extention}.{string.Join("_", Parts)}.{size}.{hash}.filebit";
            FullName = Path.Combine(Info.Directory.FullName, name);
            File.Move(Info.FullName, FullName);
            Info = new FileInfo(FullName);
            IsTemp = false;
        }

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