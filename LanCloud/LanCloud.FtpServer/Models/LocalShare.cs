using LanCloud.Shared.Log;
using LanCloud.Servers.Wjp;
using System;
using System.Net;
using LanCloud.Handlers;
using LanCloud.Configs;

namespace LanCloud.Models
{
    public class LocalShare : IDisposable
    {
        public LocalShare(IPAddress ipAdress, int port, LocalShareConfig shareConfig, ILogger logger)
        {
            IpAdress = ipAdress;
            Port = port;
            Config = shareConfig;
            Logger = logger;

            ShareHandler = new LocalShareHandler(shareConfig, Logger);
            ShareServer = new WjpServer(ipAdress, port, ShareHandler, Logger);
        }

        public IPAddress IpAdress { get; }
        public int Port { get; }
        public LocalShareConfig Config { get; }
        public LocalShareHandler ShareHandler { get; }
        public WjpServer ShareServer { get; }
        public ILogger Logger { get; }

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