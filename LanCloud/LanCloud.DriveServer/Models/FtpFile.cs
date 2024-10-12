using System;
using System.IO;

namespace LanCloud.Models
{
    public class FtpFile
    {
        public FtpFile(FileInfo fileInfo)
        {
            FileInfo = fileInfo;
        }

        public FileInfo FileInfo { get; }

        public string Name => FileInfo.Name;
        public long Length => FileInfo.Length;
        public DateTime LastWriteTime => FileInfo.LastWriteTime;
    }
}