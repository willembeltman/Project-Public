using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace LanCloud.Shared.Log
{
    public class Logger : ILogger, IDisposable
    {
        public Logger(string fullname)
        {
            Fullname = fullname;
            Thread = new Thread(new ThreadStart(Start));
            Thread.Start();
        }

        private void Start()
        {
            while (true)
            {
                if (LogReceived.WaitOne(1000))
                {
                    List<string> list = new List<string>();
                    while (Queue.TryDequeue(out var line))
                    {
                        list.Add(line);
                    }
                    if (list.Any())
                    {
                        Write(list);
                    }
                }
            }
        }

        private void Write(List<string> list)
        {
            using (var stream = File.Open(Fullname, FileMode.OpenOrCreate))
            using (var writer = new StreamWriter(stream))
            {
                stream.Position = stream.Length;
                foreach (var item in list)
                {
                    writer.WriteLine(item);
                    Console.WriteLine(item);
                }
            }
        }

        AutoResetEvent LogReceived = new AutoResetEvent(false);
        public string Fullname { get; }
        public Thread Thread { get; }
        public ConcurrentQueue<string> Queue { get; } = new ConcurrentQueue<string>();


        public void Error(string message)
        {
            var line = GetLine(message, true);
            Queue.Enqueue(line);
            LogReceived.Set();
        }

        public void Info(string message)
        {
            var line = GetLine(message, false);
            Queue.Enqueue(line);
            LogReceived.Set();
        }

        private string GetLine(string message, bool error)
        {
            var caller = GetCallerName();
            return $"{DateTime.Now.ToString("HH:mm:ss.fff")} {caller}: {(error ? "ERROR" : "")} {message}";
        }
        static string GetCallerName()
        {
            StackTrace stackTrace = new StackTrace();
            // De tweede entry in de stack trace is de aanroepende functie
            var frame = stackTrace.GetFrame(3);
            var method = frame.GetMethod();
            var methodname = method.Name == ".ctor" ? method.Name : "." + method.Name;

            return $"{method.DeclaringType.Namespace}.{method.DeclaringType.Name}{methodname}";
        }

        public void Dispose()
        {
            Thread.Abort();
        }
    }
}
