using System;

namespace LanCloud.FtpServer.Interfaces
{
    public interface IFtpFile
    {
        string Name { get; }
        long Length { get; }
        DateTime LastWriteTime { get; }
    }
}