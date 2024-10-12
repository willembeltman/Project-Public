using System;

namespace LanCloud.FtpServer
{
    public interface IFileInfo
    {
        string Name { get; }
        long Length { get; }
        DateTime LastWriteTime { get; }
    }
}