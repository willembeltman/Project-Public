using System;
using System.IO;
using System.Threading.Tasks;

namespace LanCloud
{
    internal class LoggedStreamReader
    {
        public LoggedStreamReader(StreamWriter log, StreamReader reader, string ipAdress)
        {
            Log = log;
            Reader = reader;
            IpAdress = ipAdress;
        }

        public StreamWriter Log { get; }
        public StreamReader Reader { get; }
        public string IpAdress { get; }

        internal async Task<string> ReadLineAsync()
        {
            var line = await Reader.ReadLineAsync();
            lock (Log)
            {
                Log.WriteLine($"FROM {IpAdress}: {line}");
                Log.Flush();
            }
            return line;
        }
    }
}