using LanCloud.Domain.Application;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace LanCloud.Domain.IO
{
    public class FileBitReader : IDisposable
    {
        public FileBitReader(FtpStreamReader ftpStreamReader, FileRefBit fileRefBit)
        {
            FtpStreamReader = ftpStreamReader;
            FileRefBit = fileRefBit;
            Buffer = new DoubleBuffer();

            Thread = new Thread(new ThreadStart(Start));
            Thread.Start();
        }

        public FtpStreamReader FtpStreamReader { get; }
        public FileRefBit FileRefBit { get; }
        public DoubleBuffer Buffer { get; }
        private Thread Thread { get; }
        private AutoResetEvent StartNext { get; } = new AutoResetEvent(true);
        private AutoResetEvent BufferIsWritten { get; } = new AutoResetEvent(false);
        public bool EndOfFile { get; private set; } = false;

        public int[] Indexes => FileRefBit.Indexes;
        public FileRef FileRef => FtpStreamReader.FtpFileInfo.FileRef;
        public LocalApplication Application => FtpStreamReader.FtpFileInfo.Application;

        private void Start()
        {
            var fileBit = Application
                .FindFileBits(FileRef, FileRefBit)
                .FirstOrDefault();
            if (fileBit != null)
            {
                using (var stream = fileBit.OpenRead())
                {
                    while (!EndOfFile)
                    {
                        if (StartNext.WaitOne(100))
                        {
                            Buffer.WriteBufferPosition = stream.Read(Buffer.WriteBuffer, 0, Buffer.WriteBuffer.Length);
                            if (Buffer.WriteBufferPosition <= 0)
                                EndOfFile = true;

                            BufferIsWritten.Set();
                        }
                    }
                }
            }
        }

        public void FlipBuffer()
        {
            if (!EndOfFile)
            {
                BufferIsWritten.WaitOne();
                Buffer.Flip();
                StartNext.Set();
            }
        }

        public void Dispose()
        {
            EndOfFile = true;
            if (Thread.CurrentThread != Thread)
                Thread.Join();
        }

    }
}