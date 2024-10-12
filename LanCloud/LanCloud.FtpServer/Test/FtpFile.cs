using LanCloud.FtpServer.Interfaces;
using System;
using System.IO;

namespace LanCloud.Application
{
    internal class FtpFile : IFtpFile
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