using System;
using System.IO;

namespace LanCloud.FtpServer
{
    internal class TestFile : IFileInfo
    {
        public TestFile(FileInfo fileInfo)
        {
            FileInfo = fileInfo;
        }

        public FileInfo FileInfo { get; }

        public string Name => FileInfo.Name;
        public long Length => FileInfo.Length;
        public DateTime LastWriteTime => FileInfo.LastWriteTime;
    }
}