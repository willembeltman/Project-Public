using System;
using System.IO;
using System.Threading.Tasks;

namespace LanCloud
{
    internal class LogReader
    {
        public LogReader(StreamWriter log, StreamReader reader)
        {
            Log = log;
            Reader = reader;
        }

        public StreamWriter Log { get; }
        public StreamReader Reader { get; }

        internal async Task<string> ReadLineAsync()
        {
            var line = await Reader.ReadLineAsync();
            lock (Log)
            {
                Log.WriteLine(line);
                Log.Flush();
            }
            return line;
        }
    }
}