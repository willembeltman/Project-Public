//using LanCloud.Interfaces;
//using System;
//using System.Collections.Concurrent;
//using System.Threading;
//using System.Threading.Tasks;

//namespace LanCloud.Logger
//{
//    public class LoggerService : DokanNet.Logging.ILogger, IStarteble
//    {
//        public LoggerService(App app)
//        {
//            App = app;
//            Queue = new ConcurrentQueue<string>();
//            LogUpdated = new AutoResetEvent(false);
//        }

//        public bool DebugEnabled { get; set; }
//        public App App { get; }
//        AutoResetEvent LogUpdated { get; }
//        ConcurrentQueue<string> Queue { get; }

//        public void Debug(string message, params object[] args) 
//            => WriteLine($"Debug: {message} \r\n {string.Join(", ", args)}");

//        public void Error(string message, params object[] args) 
//            => WriteLine($"Error: {message} \r\n {string.Join(", ", args)}");

//        public void Fatal(string message, params object[] args)
//            => WriteLine($"Fatal: {message} \r\n {string.Join(", ", args)}");

//        public void Info(string message, params object[] args)
//            => WriteLine($"Info: {message} \r\n {string.Join(", ", args)}");

//        public void Warn(string message, params object[] args)
//            => WriteLine($"Warn: {message} \r\n {string.Join(", ", args)}");

//        public void WriteLine(string msg)
//        {
//            Queue.Enqueue($"{DateTime.Now} {msg}");
//            LogUpdated.Set();
//        }

//        public async Task StartAsync()
//        {
//            await Task.Run(() =>
//            {
//                while (!App.KillSwitch)
//                {
//                    if (LogUpdated.WaitOne(1000))
//                    {
//                        while (!Queue.TryDequeue(out string result))
//                        {
//                            Console.WriteLine(result);
//                        }
//                    }
//                }
//            });
//        }
//    }
//}
