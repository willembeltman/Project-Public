using LanCloud.Domain.Collections;
using LanCloud.Domain.IO;
using LanCloud.Models.Configs;
using LanCloud.Servers.Wjp;
using LanCloud.Shared.Log;
using System;
using System.Linq;
using System.Net;

namespace LanCloud.Domain.Application
{
    public class LocalApplication : IDisposable
    {
        public LocalApplication(
            ApplicationConfig config,
            ILogger logger)
        {
            Config = config;
            Logger = logger;

            Authentication = new AuthenticationService(this, logger);
            FileRefs = new FileRefCollection(this, logger);
            LocalShares = new LocalShareCollection(this, logger);
            RemoteApplications = new RemoteApplicationCollection(this, logger);
            RemoteShares = new RemoteShareCollection(this, logger);

            ServerConfig = config.Servers.FirstOrDefault(a => a.IsThisComputer);
            if (ServerConfig != null)
            {
                ServerHandler = new LocalApplicationHandler(this, LocalShares, logger);
                Server = new WjpServer(IPAddress.Any, config.StartPort, ServerHandler, logger);
                
                Logger.Info($"Loaded");
            }
            else
            {
                Logger.Info($"Loaded without server");
            }
        }

        public ApplicationConfig Config { get; }
        public ILogger Logger { get; }

        public AuthenticationService Authentication { get; }
        public FileRefCollection FileRefs { get; }
        public LocalShareCollection LocalShares { get; }
        public RemoteApplicationCollection RemoteApplications { get; }
        public RemoteShareCollection RemoteShares { get; }

        public RemoteApplicationConfig ServerConfig { get; }        
        public LocalApplicationHandler ServerHandler { get; }
        public WjpServer Server { get; }

        public void Dispose()
        {
            Server.Dispose();
            LocalShares.Dispose();
        }

    }
}