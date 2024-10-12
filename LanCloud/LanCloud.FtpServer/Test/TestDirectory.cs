using LanCloud.FtpServer.Interfaces;
using System;
using System.IO;

namespace LanCloud.Application
{
    internal class TestDirectory : IDirectoryInfo
    {
        public TestDirectory(DirectoryInfo directoryInfo)
        {
            DirectoryInfo = directoryInfo;
        }

        private DirectoryInfo DirectoryInfo { get; }

        public string Name => DirectoryInfo.Name;
        public DateTime LastWriteTime => DirectoryInfo.LastWriteTime;
    }
}