using System;
using System.IO;

namespace LanCloud.FtpServer
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