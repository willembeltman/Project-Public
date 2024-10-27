using LanCloud.Models;
using LanCloud.Shared.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LanCloud
{
    public class RemoteApplicationProxyCollection : IEnumerable<RemoteApplicationProxy>, IDisposable
    {
        public RemoteApplicationProxyCollection(Config config)
        {
            ApplicationProxies = config.Servers
                .Where(a => a.IsThisComputer == false)
                .Select(serverName => new RemoteApplicationProxy(serverName))
                .ToArray();
        }

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