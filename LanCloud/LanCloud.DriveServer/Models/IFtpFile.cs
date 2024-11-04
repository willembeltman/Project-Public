using System;

namespace LanCloud.Models
{
    public interface IFtpFile
    {
        string Name { get; }
        long? Length { get; }
        DateTime LastWriteTime { get; }
    }
}