using System;

namespace SharpFtpServer
{
    public interface ILogger : IDisposable
    {
        void Error(string message);
        void Info(string message);
        void Info(LogEntry logEntry);
    }
}