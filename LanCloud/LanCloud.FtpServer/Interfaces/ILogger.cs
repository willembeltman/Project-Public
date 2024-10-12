using System;

namespace LanCloud.FtpServer
{
    public interface ILogger : IDisposable
    {
        void Error(string message);
        void Info(string message);
    }
}