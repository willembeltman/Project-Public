using LanCloud.Shared.Log;
using System;

namespace LanCloud.Collections
{
    public class RemoteShareCollection : IDisposable
    {
        public RemoteShareCollection(RemoteApplicationProxyCollection applicationProxies, ILogger logger)
        {
            ApplicationProxies = applicationProxies;
            Logger = logger;

            Logger.Info($"Loaded");
        }

        public RemoteApplicationProxyCollection ApplicationProxies { get; }
        public ILogger Logger { get; }

        public void Dispose()
        {
            ApplicationProxies.Dispose();
        }
    }
}