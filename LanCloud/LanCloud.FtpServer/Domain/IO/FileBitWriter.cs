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
        public FileBitWriter(FileRefWriter ftpStreamWriter, LocalSharePart sharePart, ILogger logger)
        {
            FtpStreamWriter = ftpStreamWriter;
            SharePart = sharePart;
            Logger = logger;

            FileBit = Share.FileBits.CreateTempFileBit(FtpStreamWriter.PathInfo.Extention, Indexes);
            Buffer = new byte[Constants.BufferSize];

            Thread = new Thread(new ThreadStart(Start));
            Thread.Start();

            //Logger.Info($"Opened {FileBit.Info.Name} as output for parts: {string.Join(" xor ", Indexes.Select(a => $"#{a}"))}");
        }

        public FileRefWriter FtpStreamWriter { get; }
        public LocalSharePart SharePart { get; }
        public ILogger Logger { get; }
        public Thread Thread { get; }
        public AutoResetEvent ReadingIsDone { get; } = new AutoResetEvent(true);
        public AutoResetEvent StartNext { get; } = new AutoResetEvent(false);
        private bool KillSwitch { get; set; } = false;
        public LocalShare Share => SharePart.Share;
        public int[] Indexes => SharePart.Part.Indexes;

        public byte[] Buffer { get; }
        public FileBit FileBit { get; private set; }
        public int Position { get; private set; }

        private void Start()
        {
            WriteAllData();
            FileBit.Update(FtpStreamWriter.Position, FtpStreamWriter.GeneratedHash);
            Share.FileBits.AddFileBit(FileBit);
        }

        private void WriteAllData()
        {
            using (var stream = FileBit.OpenWrite())
            {
                while (!KillSwitch)
                {
                    if (StartNext.WaitOne(1000))
                    {
                        if (!KillSwitch)
                        {
                            var data = FtpStreamWriter.Buffer.ReadBuffer;
                            var datalength = FtpStreamWriter.Buffer.ReadBufferPosition;
                            var width = FtpStreamWriter.Buffer.Width;

                            WriteBufferToStream(stream, data, datalength, width);

                            ReadingIsDone.Set();
                        }
                    }
                }
            }
        }

        private void WriteBufferToStream(Stream stream, byte[] data, int datalength, int width)
        {
            //Logger.Info($"Start writing to {FileBit.Info.Name}");

            // Use a floating point number to calculate the buffer size,
            // This allows for half bytes which will be calculated to 
            // real byte length when XOR is done
            var sublength = Convert.ToDouble(datalength) / width;

            // In the foreach we calculate the real byte length, this can vary
            // with half numbers so we need to calculate the maximum amount of
            // bytes used in the buffers
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

            //Thread.Sleep(100);
            //Logger.Info($"Written {maxlength}b to {FileBit.Info.Name}");
        }

        public FileBit Stop()
        {
            KillSwitch = true;
            StartNext.Set();
            Thread.Join();
            return FileBit;
        }
    }
}