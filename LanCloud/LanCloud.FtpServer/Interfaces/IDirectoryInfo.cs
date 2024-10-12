using System;

namespace LanCloud.FtpServer
{
    public interface IDirectoryInfo
    {
        string Name { get; }
        DateTime LastWriteTime { get; }
    }
}