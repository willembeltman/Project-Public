using LanCloud.Models;
using System;

namespace LanCloud
{
    internal class ShareProxyCollection : IDisposable
    {
        public ShareProxyCollection(ApplicationProxyCollection applicationProxies)
        {
            ApplicationProxies = applicationProxies;
        }

        public ApplicationProxyCollection ApplicationProxies { get; }

        public void Dispose()
        {
            ApplicationProxies.Dispose();
        }


    }
}