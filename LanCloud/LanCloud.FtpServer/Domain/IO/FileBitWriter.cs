using LanCloud.Domain.Share;
using LanCloud.Domain.VirtualFtp;
using LanCloud.Shared.Log;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace LanCloud.Domain.IO
{
    public class FileBitWriter
    {
        public FileBitWriter(FileRefWriter fileRefWriter, LocalSharePart localShare, ILogger logger)
        {
            FileRefWriter = fileRefWriter;
            LocalSharePart = localShare;
            Logger = logger;

            FileBit = LocalSharePart.FileBits.CreateTempFileBit(FileRefWriter.PathInfo.Extention);
            Buffer = new byte[Constants.BufferSize];

            Thread = new Thread(new ThreadStart(Start));
            Thread.Start();

            Logger.Info($"Opened {FileBit.Info.Name} as output for parts: {string.Join(" xor ", Indexes.Select(a => $"#{a}"))}");
        }

        public FileRefWriter FileRefWriter { get; }
        public LocalSharePart LocalSharePart { get; }
        public ILogger Logger { get; }

        public FileBit FileBit { get; }
        public byte[] Buffer { get; }
        public Thread Thread { get; }

        public AutoResetEvent WritingIsDone { get; } = new AutoResetEvent(true);
        public AutoResetEvent StartNext { get; } = new AutoResetEvent(false);
        public int Position { get; private set; } = 0;
        private bool KillSwitch { get; set; } = false;

        public byte[] Indexes => LocalSharePart.Indexes;

        private void Start()
        {
            using (var stream = FileBit.OpenWrite())
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

                            WriteBufferToStream(stream, data, datalength, width);
                        }

                        WritingIsDone.Set();
                    }
                }
            }
        }

        private void WriteBufferToStream(Stream stream, byte[] data, int datalength, int width)
        {
            var sublength = Convert.ToDouble(datalength) / width;
            var maxlength = 0;

            if (Indexes.Length == 1)
            {
                // Just write the data to the stream
                var index = Indexes.First();
                var start = Convert.ToInt32(sublength * index);
                var end = Convert.ToInt32(sublength * (index + 1));
                maxlength = end - start;
                stream.Write(data, start, maxlength);
            }
            else
            {
                // Prepare own buffer
                Array.Clear(Buffer, 0, Buffer.Length);

                // XOR data from indexes on to own buffer
                foreach (var index in Indexes)
                {
                    var start = Convert.ToInt32(sublength * index);
                    var end = Convert.ToInt32(sublength * (index + 1));
                    var length = end - start;
                    if (length > maxlength) maxlength = length;

                    for (var i = 0; i < length; i++)
                    {
                        Buffer[i] ^= data[start + i];
                    }
                }

                // Then write own buffer to disk
                stream.Write(Buffer, 0, maxlength);
            }

            Position += maxlength;
        }

        public FileBit Stop(long length, string hash)
        {
            KillSwitch = true;
            StartNext.Set();
            Thread.Join();

            FileBit.Update(length, hash);
            LocalSharePart.FileBits.AddFileBit(FileBit);
            return FileBit;
        }
    }
}