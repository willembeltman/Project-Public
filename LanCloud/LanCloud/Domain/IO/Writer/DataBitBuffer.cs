using LanCloud.Domain.Share;
using LanCloud.Models;
using LanCloud.Shared.Log;
using System;
using System.Linq;
using System.Threading;

namespace LanCloud.Domain.IO.Writer
{
    public class DataBitBuffer
    {
        public DataBitBuffer(FileRefWriter fileRefWriter, byte index, LocalSharePart[] localShareParts, ILogger logger)
        {
            FileRefWriter = fileRefWriter;
            Index = index;
            LocalShareParts = localShareParts;
            Logger = logger;

            Buffer = new DoubleBuffer(1);
            FileBitWriters = localShareParts
                .Select(localSharePart => new FileBitWriter(Buffer, fileRefWriter, localSharePart, logger))
                .ToArray();

            Thread = new Thread(new ThreadStart(Start));
            Thread.Start();
        }

        public FileRefWriter FileRefWriter { get; }
        public byte Index { get; }
        public LocalSharePart[] LocalShareParts { get; }
        public ILogger Logger { get; }

        public DoubleBuffer Buffer { get; }
        public FileBitWriter[] FileBitWriters { get; }

        public Thread Thread { get; }

        public AutoResetEvent WritingIsDone { get; } = new AutoResetEvent(true);
        public AutoResetEvent StartNext { get; } = new AutoResetEvent(false);
        private bool KillSwitch { get; set; } = false;

        //public FileBit FileBit => FileBitWriter.FileBit;

        private void Start()
        {
            while (!KillSwitch)
            {
                if (StartNext.WaitOne(1000))
                {
                    if (!KillSwitch && FileRefWriter.Buffer.ReadBufferPosition > 0)
                    {
                        var data = FileRefWriter.Buffer.ReadBuffer;
                        var datalength = FileRefWriter.Buffer.ReadBufferPosition;
                        var width = FileRefWriter.Buffer.Width;

                        WriteBufferToStream(data, datalength, width);
                    }

                    WritingIsDone.Set();
                }
            }
        }

        private void WriteBufferToStream(byte[] data, int datalength, int width)
        {
            var sublength = Convert.ToDouble(datalength) / width;
            var start = Convert.ToInt32(sublength * Index);
            var end = Convert.ToInt32(sublength * (Index + 1));
            var length = end - start;

            Array.Copy(data, start, Buffer.WriteBuffer, 0, length);
            Buffer.WriteBufferPosition = length;

            FlipBuffer();
        }

        public void FlipBuffer()
        {
            foreach (var fileBitWriter in FileBitWriters)
                if (!fileBitWriter.WritingIsDone.WaitOne(100000))
                    throw new Exception("Timeout writing to FileBitWriter");

            Buffer.Flip();

            foreach (var fileBitWriter in FileBitWriters)
                fileBitWriter.StartNext.Set();
        }

        public FileBit[] Stop(long length, string hash)
        {
            if (Thread.CurrentThread == Thread) throw new Exception("Cannot wait for own thread");

            KillSwitch = true;
            StartNext.Set();
            Thread.Join();

            return FileBitWriters.Select(a => a.Stop(length, hash)).ToArray();
        }
    }
}