namespace SharpFtpServer
{
    internal interface ILogger
    {
        void Error(string message);
        void Info(string message);
        void Info(LogEntry logEntry);
    }
}