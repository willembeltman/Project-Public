using LanCloud.Domain.Share;
using System;
using System.Data;
using System.IO;
using System.Linq;

namespace LanCloud.Domain.IO
{
    public class FileBit : IFileBit
    {
        public FileBit(FileInfo info)
        {
            Info = info;
            FullName = info.FullName;
            var split = info.Name.Split('.');
            Extention = split[0];
            Hash = split[1];
            Length = Convert.ToInt64(split[2]);
            Indexes = split[3].Split('_').Select(a => Convert.ToInt32(a)).ToArray();
        }
        public FileBit(LocalSharePart dirinfo, string extention, int[] indexes)
        {
            Extention = extention;
            Indexes = indexes;
            var name = $"{extention}.{Guid.NewGuid().ToString()}.{string.Join("_", indexes)}.tempbit";
            FullName = Path.Combine(dirinfo.FileBits.Root.FullName, name);
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
            var name = $"{Extention}.{hash}.{length}.{string.Join("_", Indexes)}.filebit";
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