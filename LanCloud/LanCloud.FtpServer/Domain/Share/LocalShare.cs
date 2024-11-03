using LanCloud.Shared.Log;
using LanCloud.Servers.Wjp;
using System;
using System.Net;
using LanCloud.Models.Configs;
using LanCloud.Domain.IO;
using System.Linq;

namespace LanCloud.Domain.Share
{
    public class LocalShare : IDisposable
    {
        public LocalShare(IPAddress ipAdress, int port, ApplicationConfig config, LocalShareConfig shareConfig, ILogger logger)
        {
            IpAdress = ipAdress;
            Port = port;
            Config = shareConfig;
            Logger = logger;

            Storage = new LocalShareStorage(shareConfig, logger);
            Handler = new LocalShareHandler(Storage, Logger);
            Server = new WjpServer(ipAdress, port, Handler, Logger);

            ShareParts = Config.Parts
                .Select(part => new LocalSharePart(this, part))
                .ToArray();

            Logger.Info($"Loaded");
        }

        public IPAddress IpAdress { get; }
        public int Port { get; }
        public LocalShareConfig Config { get; }
        public ILogger Logger { get; }
        public LocalShareStorage Storage { get; }
        public LocalShareHandler Handler { get; }
        public WjpServer Server { get; }
        public LocalSharePart[] ShareParts { get; }

        public void Dispose()
        {
            Server.Dispose();
        }

        public int GetFreeSpace()
        {
            return 0;
        }
    }
}