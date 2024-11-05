using LanCloud.Domain.VirtualFtp;
using LanCloud.Models;
using LanCloud.Shared.Log;
using System;
using System.Threading;

namespace LanCloud.Domain.IO
{
    public class FileBitReader : IDisposable
    {
        public FileBitReader(FtpStreamReader streamReader, FileBit fileBit, ILogger logger)
        {
            StreamReader = streamReader;
            FileBit = fileBit;
            Logger = logger;

            Buffer = new DoubleBuffer();

            Thread = new Thread(new ThreadStart(Start));
            Thread.Start();
        }

        public FtpStreamReader StreamReader { get; }
        public FileBit FileBit { get; }
        public ILogger Logger { get; }
        public DoubleBuffer Buffer { get; }
        private Thread Thread { get; }
        private AutoResetEvent StartNext { get; } = new AutoResetEvent(true);
        private AutoResetEvent BufferIsWritten { get; } = new AutoResetEvent(false);
        private bool EndOfFile { get; set; } = false;

        public int[] Indexes => FileBit.Indexes;

        public bool KillSwitch { get; private set; }

        private void Start()
        {
            if (FileBit != null)
            {
                using (var stream = FileBit.OpenRead())
                {
                    while (!KillSwitch)
                    {
                        if (StartNext.WaitOne(100))
                        {
                            if (!EndOfFile)
                            {
                                Buffer.WriteBufferPosition = stream.Read(Buffer.WriteBuffer, 0, Buffer.WriteBuffer.Length);
                                if (Buffer.WriteBufferPosition <= 0)
                                    EndOfFile = true;
                            }
                            else
                            {
                                Buffer.WriteBufferPosition = 0;
                            }

                            BufferIsWritten.Set();
                        }
                    }
                }
            }
        }

        public void FlipBuffer()
        {
            BufferIsWritten.WaitOne();
            
            Buffer.Flip();

            StartNext.Set();
        }

        public void Dispose()
        {
            KillSwitch = true;
            if (Thread.CurrentThread != Thread)
                Thread.Join();
        }

    }
}