using LanCloud.Collections;
using LanCloud.Models.Configs;
using LanCloud.Servers.Wjp;
using LanCloud.Shared.Log;
using System;
using System.Net;

namespace LanCloud.Domain.Application
{
    public class LocalApplication : IDisposable
    {
        public LocalApplication(
            ApplicationConfig config, 
            LocalShareCollection shares, 
            RemoteApplicationProxyCollection remoteApplications, 
            RemoteShareCollection remoteShares,
            ILogger logger)
        {
            Config = config;
            Shares = shares;
            RemoteApplications = remoteApplications;
            RemoteShares = remoteShares;
            Logger = logger;

            Authentication = new AuthenticationService(this, logger);
            ApplicationHandler = new LocalApplicationHandler(this, shares, logger);
            ApplicationServer = new WjpServer(IPAddress.Any, config.StartPort, ApplicationHandler, logger);

            Logger.Info($"Loaded");
        }

        public ApplicationConfig Config { get; }
        public LocalShareCollection Shares { get; }
        public RemoteApplicationProxyCollection RemoteApplications { get; }
        public RemoteShareCollection RemoteShares { get; }
        public ILogger Logger { get; }
        public AuthenticationService Authentication { get; }
        public LocalApplicationHandler ApplicationHandler { get; }
        public WjpServer ApplicationServer { get; }

        public void Dispose()
        {
            ApplicationServer.Dispose();
        }
    }
}