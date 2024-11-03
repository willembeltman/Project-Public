using LanCloud.Domain.Share;
using LanCloud.Models.Configs;
using System;
using System.IO;
using System.Threading;

namespace LanCloud.Domain.IO
{
    public class VirtualStreamWriterSharePart : IDisposable
    {
        public VirtualStreamWriterSharePart(VirtualStreamWriter virtualStreamWriter, LocalSharePart sharePart)
        {
            VirtualStreamWriter = virtualStreamWriter;
            SharePart = sharePart;

            TempFullName = Path.Combine(sharePart.Share.Config.DirectoryName, Guid.NewGuid().ToString() + ".temp");
            Stream = File.Create(TempFullName);

            Thread = new Thread(new ThreadStart(Start));
            Thread.Start();

            Buffer = new byte[Constants.BufferSize];
        }

        public VirtualStreamWriter VirtualStreamWriter { get; }
        public LocalSharePart SharePart { get; }
        public string TempFullName { get; }
        public FileStream Stream { get; }
        public Thread Thread { get; }
        public AutoResetEvent ReadingIsDone { get; } = new AutoResetEvent(false);
        public AutoResetEvent StartNext { get; } = new AutoResetEvent(false);
        private bool KillSwitch { get; set; } = false;
        public bool Disposed { get; private set; }
        public LocalShare Share => SharePart.Share;
        public LocalSharePartConfig Part => SharePart.Part;

        public byte[] Buffer { get; }

        private void Start()
        {
            while (!KillSwitch)
            {
                if (StartNext.WaitOne(1000))
                {
                    Write();
                    ReadingIsDone.Set();
                }
            }
        }

        private void Write()
        {
            var data = VirtualStreamWriter.ReadBuffer;
            var length = VirtualStreamWriter.ReadBufferWritten;
            var width = VirtualStreamWriter.Width;
            var sublength = Convert.ToDouble(length) / width;

            for (var i = 0; i < length; i++)
            {
                Buffer[i] = 0;
            }
            foreach (var index in Part.Indexes)
            {
                var start = Convert.ToInt32(sublength * index);
                var end = Convert.ToInt32(sublength * (index + 1));
                var l = end - start;
                for (var i = 0; i < l; i++)
                {
                    Buffer[i] ^= data[start + i];
                }
            }
            Stream.Write(Buffer, 0, length);    
        }

        public void Dispose()
        {
            KillSwitch = true;
            if (Thread.CurrentThread == Thread && Disposed == false)
            {
                Thread.Join();
                Stream.Dispose();
                Disposed = true;
            }
        }
    }
}