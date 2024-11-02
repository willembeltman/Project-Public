using LanCloud.Collections;
using LanCloud.Configs;
using LanCloud.Servers.Wjp;
using LanCloud.Shared.Log;
using System;
using System.Net;

namespace LanCloud.Domain.Application
{
    public class LocalApplication : IDisposable
    {
        public LocalApplication(
            Config config, 
            LocalShareCollection shares, 
            RemoteApplicationProxyCollection remoteApplications, 
            RemoteShareCollection remoteShares,
            ILogger logger)
        {
            ApplicationHandler = new LocalApplicationHandler(this, shares, logger);
            ApplicationServer = new WjpServer(IPAddress.Any, config.StartPort, ApplicationHandler, logger);
        }

        public LocalApplicationHandler ApplicationHandler { get; }
        public WjpServer ApplicationServer { get; }

        public void Dispose()
        {
            ApplicationServer.Dispose();
        }
    }
}