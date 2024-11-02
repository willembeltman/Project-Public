using LanCloud.Shared.Log;
using LanCloud.Servers.Wjp;
using System;
using System.Net;
using LanCloud.Configs;

namespace LanCloud.Domain.Share
{
    public class LocalShare : IDisposable
    {
        public LocalShare(IPAddress ipAdress, int port, LocalShareConfig shareConfig, ILogger logger)
        {
            IpAdress = ipAdress;
            Port = port;
            Config = shareConfig;
            Logger = logger;

            ShareStorage = new LocalShareStorage(shareConfig, logger);
            ShareHandler = new LocalShareHandler(shareConfig, Logger);
            ShareServer = new WjpServer(ipAdress, port, ShareHandler, Logger);
        }

        public IPAddress IpAdress { get; }
        public int Port { get; }
        public LocalShareConfig Config { get; }
        public ILogger Logger { get; }
        public LocalShareStorage ShareStorage { get; }
        public LocalShareHandler ShareHandler { get; }
        public WjpServer ShareServer { get; }

        public void Dispose()
        {
            ShareServer.Dispose();
        }

        public int GetFreeSpace()
        {
            return 0;
        }
    }
}