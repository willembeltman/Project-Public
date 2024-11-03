using LanCloud.Domain.Application;
using LanCloud.Models.Configs;
using LanCloud.Shared.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LanCloud.Collections
{
    public class RemoteApplicationProxyCollection : IEnumerable<RemoteApplicationProxy>, IDisposable
    {
        public RemoteApplicationProxyCollection(ApplicationConfig config, ILogger logger)
        {
            Config = config;
            Logger = logger;
            ApplicationProxies = config.Servers
                .Where(a => a.IsThisComputer == false)
                .Select(remoteconfig => new RemoteApplicationProxy(remoteconfig, logger))
                .ToArray();

            Logger.Info("Loaded");
        }

        public ApplicationConfig Config { get; }
        public ILogger Logger { get; }
        public RemoteApplicationProxy[] ApplicationProxies { get; }

        public void Dispose()
        {
            foreach (var externalApplication in ApplicationProxies)
            {
                externalApplication.Dispose();
            };
        }

        public IEnumerator<RemoteApplicationProxy> GetEnumerator()
        {
            return ((IEnumerable<RemoteApplicationProxy>)ApplicationProxies).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ApplicationProxies.GetEnumerator();
        }
    }
}