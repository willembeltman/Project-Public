using LanCloud.Models;
using System;

namespace LanCloud
{
    public class RemoteShareCollection : IDisposable
    {
        public RemoteShareCollection(RemoteApplicationProxyCollection applicationProxies)
        {
            ApplicationProxies = applicationProxies;
        }

        public RemoteApplicationProxyCollection ApplicationProxies { get; }

        public void Dispose()
        {
            ApplicationProxies.Dispose();
        }


    }
}