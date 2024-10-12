using System;

namespace LanCloud.FtpServer.Interfaces
{
    public interface IDirectoryInfo
    {
        string Name { get; }
        DateTime LastWriteTime { get; }
    }
}