using System;

namespace LanCloud.FtpServer.Interfaces
{
    public interface IFtpDirectory
    {
        string Name { get; }
        DateTime LastWriteTime { get; }
    }
}