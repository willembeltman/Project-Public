using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace LanCloud
{
    internal class LoggedStreamWriter
    {
        public LoggedStreamWriter(StreamWriter log, StreamWriter writer, string ipAdress)
        {
            Log = log;
            Writer = writer;
            IpAdress = ipAdress;
        }

        public StreamWriter Log { get; }
        public StreamWriter Writer { get; }
        public string IpAdress { get; }

        internal async Task WriteLineAsync(string line)
        {
            await Writer.WriteLineAsync(line);
            lock (Log)
            {
                Log.WriteLine($"TO {IpAdress}: {line}");
                Log.Flush();
            }
        }
    }
}