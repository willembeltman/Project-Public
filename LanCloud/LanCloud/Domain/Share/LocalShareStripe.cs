using LanCloud.Shared.Log;
using System;
using LanCloud.Models.Configs;
using System.Linq;

namespace LanCloud.Domain.Share
{
    public class LocalShareStripe : IShareStripe
    {
        public LocalShareStripe(LocalShare localShare, LocalShareBitConfig partConfig, ILogger logger)
        {
            LocalShare = localShare;
            PartConfig = partConfig;
            Logger = logger;

            Indexes = PartConfig.Indexes.Select(a => Convert.ToByte(a)).ToArray();
        }

        public LocalShare LocalShare { get; }
        public LocalShareBitConfig PartConfig { get; }
        public ILogger Logger { get; }

        public byte[] Indexes { get; }

        public IShare Share => LocalShare;

        //public bool SaveFile(FileBit bit, IEnumerable<SingleBuffer> datareader)
        //{
        //    if (datareader == null) return false;

        //    using (var stream = bit.OpenWrite())
        //    {
        //        foreach (var data in datareader)
        //        {
        //            stream.Write(data.Buffer, 0, data.BufferPosition);
        //        }
        //    }

        //    return true;
        //}
        //public IEnumerable<SingleBuffer> LoadFile(string path)
        //{
        //    var fileinfo = new PathFileInfo(Application, path, Logger);
        //    if (!fileinfo.Exists) return null;
        //    return LoadFileYield(fileinfo);
        //}

        //private IEnumerable<SingleBuffer> LoadFileYield(PathFileInfo fileinfo)
        //{
        //    SingleBuffer buffer = new SingleBuffer();
        //    using (var stream = fileinfo.OpenRead())
        //    {
        //        while ((buffer.BufferPosition = stream.Read(buffer.Buffer, 0, buffer.Length)) > 0)
        //        {
        //            yield return buffer;
        //        }
        //    }
        //}

    }
}