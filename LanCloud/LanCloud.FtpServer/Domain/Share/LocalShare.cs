using LanCloud.Shared.Log;
using LanCloud.Models.Configs;
using LanCloud.Domain.Collections;
using LanCloud.Domain.Application;
using System.Linq;
using System.Collections.Generic;

namespace LanCloud.Domain.Share
{
    public class LocalShare
    {
        private string _Status { get; set; }
        public string Status
        {
            get => _Status;
            set
            {
                _Status = value;
                Application.StatusChanged();
            }
        }

        public LocalShare(LocalShareCollection localShares, LocalShareConfig shareConfig, ref int port, ILogger logger)
        {
            LocalShares = localShares;
            ShareConfig = shareConfig;
            Logger = logger;

            var list = new List<LocalSharePart>();
            foreach (var part in shareConfig.Parts)
            {
                port = port + 1;
                list.Add(new LocalSharePart(this, part, HostName, port, logger));
            }
            LocalShareParts = list.ToArray();

            FileBits = new FileBitCollection(this, Logger);

            Status = Logger.Info($"OK");
        }

        public LocalShareCollection LocalShares { get; }
        public LocalShareConfig ShareConfig { get; }
        public ILogger Logger { get; }
        public LocalSharePart[] LocalShareParts { get; }
        public FileBitCollection FileBits { get; }

        public LocalApplication Application => LocalShares.Application;
        public string HostName => Application.HostName;

    }
}