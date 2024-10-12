using LanCloud.Handlers;
using LanCloud.Servers.Application;
using System;
using System.Net;

namespace LanCloud.Models
{
    internal class Application : IDisposable
    {
        public Application(
            ApplicationConfig config,
            ShareCollection shares, 
            ApplicationProxyCollection applicationProxies, 
            ShareProxyCollection shareProxies)
        {
            ApplicationHandler = new ApplicationHandler(shares, applicationProxies, shareProxies);
            ApplicationServer = new ApplicationServer(IPAddress.Any, config.Port, ApplicationHandler);
        }

        public ApplicationHandler ApplicationHandler { get; }
        public ApplicationServer ApplicationServer { get; }

        public void Dispose()
        {
            ApplicationServer.Dispose();
        }
    }
}