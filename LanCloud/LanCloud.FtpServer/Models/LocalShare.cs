using LanCloud.Servers.Share;
using LanCloud.Shared.Models;
using System;
using System.Net;

namespace LanCloud.Models
{
    public class LocalShare : IDisposable
    {
        public LocalShare(IPAddress ipAdress, int port, LocalShareConfig shareConfig)
        {
            IpAdress = ipAdress;
            Port = port;
            Config = shareConfig;
            ShareHandler = new LocalShareHandler(shareConfig);
            ShareServer = new LocalShareServer(ipAdress, port, ShareHandler);
        }

        public IPAddress IpAdress { get; }
        public int Port { get; }
        public LocalShareConfig Config { get; }
        public LocalShareHandler ShareHandler { get; }
        public LocalShareServer ShareServer { get; }

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