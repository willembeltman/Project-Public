using LanCloud.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LanCloud
{
    internal class ApplicationProxyCollection : IEnumerable<ApplicationProxy>, IDisposable
    {
        public ApplicationProxyCollection(ApplicationConfig config)
        {
            ApplicationProxies = config.Servers
                .Where(a => a.IsThisComputer == false)
                .Select(serverName => new ApplicationProxy(serverName))
                .ToArray();
        }

        public ApplicationProxy[] ApplicationProxies { get; }

        public void Dispose()
        {
            foreach (var externalApplication in ApplicationProxies)
            {
                externalApplication.Dispose();
            };
        }

        public IEnumerator<ApplicationProxy> GetEnumerator()
        {
            return ((IEnumerable<ApplicationProxy>)ApplicationProxies).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ApplicationProxies.GetEnumerator();
        }
    }
}