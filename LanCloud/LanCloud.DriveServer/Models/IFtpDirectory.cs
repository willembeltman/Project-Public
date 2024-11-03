using System;
namespace LanCloud.Models
{
    public interface IFtpDirectory
    {
        string Name { get; }
        DateTime LastWriteTime { get; }
    }
}