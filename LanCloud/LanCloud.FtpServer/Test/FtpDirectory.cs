using LanCloud.FtpServer.Interfaces;
using System;
using System.IO;

namespace LanCloud.Application
{
    internal class FtpDirectory : IFtpDirectory
    {
        public FtpDirectory(DirectoryInfo directoryInfo)
        {
            DirectoryInfo = directoryInfo;
        }

        private DirectoryInfo DirectoryInfo { get; }

        public string Name => DirectoryInfo.Name;
        public DateTime LastWriteTime => DirectoryInfo.LastWriteTime;
    }
}