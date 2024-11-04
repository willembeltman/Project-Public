using LanCloud.Domain.Application;
using LanCloud.Domain.Share;
using LanCloud.Models.Configs;
using System;
using System.IO;
using System.Threading;

namespace LanCloud.Domain.IO
{
    public class VirtualStreamWriterSharePart
    {
        public VirtualStreamWriterSharePart(VirtualStreamWriter virtualStreamWriter, LocalSharePart sharePart)
        {
            VirtualStreamWriter = virtualStreamWriter;
            SharePart = sharePart;
            
            Thread = new Thread(new ThreadStart(Start));
            Thread.Start();

            Buffer = new byte[Constants.BufferSize];
        }

        public VirtualStreamWriter VirtualStreamWriter { get; }
        public LocalSharePart SharePart { get; }
        public Thread Thread { get; }
        public AutoResetEvent ReadingIsDone { get; } = new AutoResetEvent(true);
        public AutoResetEvent StartNext { get; } = new AutoResetEvent(false);
        private bool KillSwitch { get; set; } = false;
        public VirtualFileInfo VirtualFileInfo => VirtualStreamWriter.VirtualFileInfo;
        public LocalShare Share => SharePart.Share;
        public LocalSharePartConfig Part => SharePart.Part;

        public byte[] Buffer { get; }
        private FileBit FileBit { get; set; }

        private void Start()
        {
            FileBit = Share.Storage.CreateTempFileBit(VirtualFileInfo.Extention, Part.Indexes);
            using (var stream = FileBit.OpenWrite())
            {
                while (!KillSwitch)
                {
                    if (StartNext.WaitOne(100))
                    {
                        var data = VirtualStreamWriter.ReadBuffer;
                        var totallength = VirtualStreamWriter.ReadBufferWritten;
                        var width = VirtualStreamWriter.Width;
                        var sublength = Convert.ToDouble(totallength) / width;
                        var maxlength = 0;
                        Array.Clear(Buffer, 0, Buffer.Length);

                        foreach (var index in Part.Indexes)
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
                        ReadingIsDone.Set();
                    }
                }
            }
            FileBit.Update(VirtualStreamWriter.Position, VirtualStreamWriter.Hash);
            Share.Storage.AddFileBit(FileBit);
        }

        public FileBit Stop()
        {
            KillSwitch = true;
            Thread.Join();
            return FileBit;
        }
    }
}