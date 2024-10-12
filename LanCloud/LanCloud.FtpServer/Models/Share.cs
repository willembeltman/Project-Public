using LanCloud.Models;
using LanCloud.Servers.Share;
using System;
using System.Net;

namespace LanCloud
{
    internal class Share : IDisposable
    {
        public Share(IPAddress ipAdress, int port, ShareConfig shareConfig)
        {
            IpAdress = ipAdress;
            Port = port;
            Config = shareConfig;
            ShareHandler = new ShareHandler(shareConfig);
            ShareServer = new ShareServer(ipAdress, port, ShareHandler);
        }

        public IPAddress IpAdress { get; }
        public int Port { get; }
        public ShareConfig Config { get; }
        public ShareHandler ShareHandler { get; }
        public ShareServer ShareServer { get; }

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