using System;

namespace LanCloud.Shared.Log
{
    public interface ILogger : IDisposable
    {
        bool LogInfo { get; set; }

        void Error(string message);
        string Info(string message);
    }
}