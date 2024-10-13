using LanCloud.Handlers;
using LanCloud.Servers.Application;
using System;
using System.Net;

namespace LanCloud.Models
{
    public class LocalApplication : IDisposable
    {
        public LocalApplication(
            Config config, 
            LocalShareCollection shares, 
            RemoteApplicationProxyCollection remoteApplications, 
            RemoteShareCollection remoteShares)
        {
            ApplicationHandler = new LocalApplicationHandler(this, shares);
            ApplicationServer = new LocalApplicationServer(IPAddress.Any, config.Port, ApplicationHandler);
        }

        public LocalApplicationHandler ApplicationHandler { get; }
        public LocalApplicationServer ApplicationServer { get; }

        public void Dispose()
        {
            ApplicationServer.Dispose();
        }
    }
}