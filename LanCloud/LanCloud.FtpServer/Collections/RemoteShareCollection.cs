using LanCloud.Shared.Log;
using System;

namespace LanCloud.Collections
{
    public class RemoteShareCollection : IDisposable
    {
        public RemoteShareCollection(RemoteApplicationProxyCollection applicationProxies)
        {
            ApplicationProxies = applicationProxies;
        }

        public RemoteShareCollection(RemoteApplicationProxyCollection applicationProxies, ILogger logger) : this(applicationProxies)
        {
            Logger = logger;
        }

        public RemoteApplicationProxyCollection ApplicationProxies { get; }
        public ILogger Logger { get; }

        public void Dispose()
        {
            ApplicationProxies.Dispose();
        }
    }
}