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
    }
}