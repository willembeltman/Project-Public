using LanCloud.Domain.Application;
using LanCloud.Domain.Share;
using LanCloud.Models.Configs;
using System;
using System.IO;
using System.Threading;

namespace LanCloud.Domain.IO
{
    public class FtpStreamWriterSharePart
    {
        public FtpStreamWriterSharePart(FtpStreamWriter ftpStreamWriter, LocalSharePart sharePart)
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
        private FileBit FileBit { get; set; }
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
                        WriteBuffer(stream);
                        ReadingIsDone.Set();
                    }
                }
            }
        }

        private void WriteBuffer(Stream stream)
        {
            var data = FtpStreamWriter.Buffer.ReadBuffer;
            var totallength = FtpStreamWriter.Buffer.ReadBufferPosition;
            var width = FtpStreamWriter.Buffer.Width;
            var sublength = Convert.ToDouble(totallength) / width;
            var maxlength = 0;
            Array.Clear(Buffer, 0, Buffer.Length);

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

            stream.Write(Buffer, 0, maxlength);
            Position += maxlength;
        }

        public FileBit Stop()
        {
            KillSwitch = true;
            Thread.Join();
            return FileBit;
        }
    }
}