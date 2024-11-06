using System;

namespace LanCloud.Shared.Log
{
    public interface ILogger : IDisposable
    {
        void Error(string message);
        string Info(string message);
    }
}