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
            Indexes = split[1].Split('_').Select(a => Convert.ToInt32(a)).ToArray();
            Length = Convert.ToInt64(split[2]);
            Hash = split[3];
        }
        public FileBit(DirectoryInfo dirinfo, string extention, int[] indexes)
        {
            Extention = extention;
            Indexes = indexes;
            var name = $"{extention}.{string.Join("_", indexes)}.{Guid.NewGuid().ToString()}.tempbit";
            FullName = Path.Combine(dirinfo.FullName, name);
            Info = new FileInfo(FullName);
            IsTemp = true;
        }

        public string Extention { get; }
        public int[] Indexes { get; }
        public FileInfo Info { get; private set; }
        public bool IsTemp { get; private set; }
        public string FullName { get; private set; }
        public long Length { get; private set; }
        public string Hash { get; private set; }

        public void Update(long length, string hash)
        {
            Length = length;
            Hash = hash;
            var name = $"{Extention}.{string.Join("_", Indexes)}.{length}.{hash}.filebit";
            FullName = Path.Combine(Info.Directory.FullName, name);
            if (File.Exists(FullName))
                File.Delete(Info.FullName);
            else
                File.Move(Info.FullName, FullName);
            Info = new FileInfo(FullName);
            IsTemp = false;
        }

        public FileStream OpenRead()
        {
            return Info.OpenRead();
        }
        public FileStream OpenWrite()
        {
            return Info.OpenWrite();
        }
    }
}