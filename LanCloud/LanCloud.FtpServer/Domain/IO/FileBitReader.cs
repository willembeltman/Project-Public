using LanCloud.Domain.VirtualFtp;
using LanCloud.Models;
using LanCloud.Shared.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace LanCloud.Domain.IO
{
    public class FileBitReader : IDisposable
    {
        public FileBitReader(FileRefReader streamReader, FileBit fileBit, ILogger logger)
        {
            if (fileBit == null)
            {
                Exception = new Exception("FileBit not found");
                return;
            }

            StreamReader = streamReader;
            FileBit = fileBit;
            Logger = logger;

            Buffer = new DoubleBuffer();

            Thread = new Thread(new ThreadStart(Start));
            Thread.Start();
        }

        public FileRefReader StreamReader { get; }
        public FileBit FileBit { get; }
        public ILogger Logger { get; }
        public DoubleBuffer Buffer { get; }
        private Thread Thread { get; }
        private AutoResetEvent StartNext { get; } = new AutoResetEvent(true);
        private AutoResetEvent BufferIsWritten { get; } = new AutoResetEvent(false);
        private bool EndOfFile { get; set; } = false;
        public bool KillSwitch { get; private set; }
        private Queue<long> Speeds { get; } = new Queue<long>(16);
        public Exception Exception { get; private set; }

        public long Speed { get; set; }
        private void AddSpeed(long speed)
        {
            while (Speeds.Count > 16)
            {
                Speeds.Dequeue();
            }
            Speeds.Enqueue(speed);
            Speed = Speeds.Sum() / Speeds.Count;
        }

        public byte[] Indexes => FileBit.Indexes;
        public int ReadCounter => Speeds.Count;

        private void Start()
        {
            if (FileBit != null)
            {
                try
                {
                    using (var stream = FileBit.OpenRead())
                    {
                        Stopwatch stopwatch = Stopwatch.StartNew();
                        while (!KillSwitch)
                        {
                            if (StartNext.WaitOne(100))
                            {
                                if (!EndOfFile)
                                {
                                    Buffer.WriteBufferPosition = stream.Read(Buffer.WriteBuffer, 0, Buffer.WriteBuffer.Length);
                                    if (Buffer.WriteBufferPosition <= 0)
                                        EndOfFile = true;

                                    var time = stopwatch.ElapsedTicks;
                                    stopwatch.Restart();
                                    var speed = Buffer.WriteBufferPosition * Stopwatch.Frequency / time;
                                    AddSpeed(speed);
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
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
        }

        public void FlipBuffer(int bufferCounter)
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