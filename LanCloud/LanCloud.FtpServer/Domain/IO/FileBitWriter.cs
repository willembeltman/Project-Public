using LanCloud.Domain.Share;
using System;
using System.IO;
using System.Threading;

namespace LanCloud.Domain.IO
{
    public class FileBitWriter
    {
        public FileBitWriter(FtpStreamWriter ftpStreamWriter, LocalSharePart sharePart)
        {
            FtpStreamWriter = ftpStreamWriter;
            SharePart = sharePart;
            
            Thread = new Thread(new ThreadStart(Start));
            Thread.Start();

            Buffer = new byte[Constants.BufferSize];
        }

        public FtpStreamWriter FtpStreamWriter { get; }
        public LocalSharePart SharePart { get; }
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
            FileBit = Share.Storage.CreateTempFileBit(FtpStreamWriter.FtpFileInfo.Extention, Indexes);
            WriteAllData();
            FileBit.Update(FtpStreamWriter.Position, FtpStreamWriter.GeneratedHash);
            Share.Storage.AddFileBit(FileBit);
        }

        private void WriteAllData()
        {
            using (var stream = FileBit.OpenWrite())
            {
                while (!KillSwitch)
                {
                    if (StartNext.WaitOne(100))
                    {
                        if (!KillSwitch)
                        {
                            WriteBuffer(stream);
                            ReadingIsDone.Set();
                        }
                    }
                }
            }
        }

        private void WriteBuffer(Stream stream)
        {
            var data = FtpStreamWriter.Buffer.ReadBuffer;
            var datalength = FtpStreamWriter.Buffer.ReadBufferPosition;
            var width = FtpStreamWriter.Buffer.Width;

            // Use a floating point number to calculate the buffer size,
            // This allows for half bytes which will be calculated to 
            // real byte length when XOR is done
            var sublength = Convert.ToDouble(datalength) / width;

            // In the foreach we calculate the real byte length, this can vary
            // with half numbers so we need to calculate the maximum amount of
            // bytes used in the buffers
            var maxlength = 0;

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
            Position += maxlength;
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