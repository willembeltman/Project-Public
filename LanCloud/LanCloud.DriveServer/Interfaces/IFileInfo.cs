using System;

namespace LanCloud.FtpServer.Interfaces
{
    public interface IFileInfo
    {
        string Name { get; }
        long Length { get; }
        DateTime LastWriteTime { get; }
    }
}