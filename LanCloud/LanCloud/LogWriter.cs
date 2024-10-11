using System;
using System.IO;
using System.Threading.Tasks;

namespace LanCloud
{
    internal class LogWriter
    {
        public LogWriter(StreamWriter log, StreamWriter writer)
        {
            Log = log;
            Writer = writer;
        }

        public StreamWriter Log { get; }
        public StreamWriter Writer { get; }

        internal async Task WriteLineAsync(string line)
        {
            await Writer.WriteLineAsync(line);
            lock (Log)
            {
                Log.WriteLine(line);
                Log.Flush();
            }
        }
    }
}