using System;

namespace LanCloud.Logger
{
    public interface ILogger : IDisposable
    {
        void Error(string message);
        void Info(string message);
    }
}