using LanCloud.Domain.Application;
using LanCloud.Domain.Share;
using LanCloud.Shared.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LanCloud.Domain.Collections
{
    public class RemoteShareCollection : IEnumerable<RemoteShare>, IDisposable
    {
        public RemoteShareCollection(LocalApplication application, ILogger logger)
        {
            Application = application;
            Logger = logger;

            RemoteShares =
                Application.RemoteApplications
                    //.Where(a => a.Connected) // Hij is nog niet connected 
                    .SelectMany(a => a.GetShares())
                    .Select(a => new RemoteShare(this, a, Logger))
                    .ToArray();
        }

        public LocalApplication Application { get; }
        public ILogger Logger { get; }
        public RemoteShare[] RemoteShares { get; }

        public void Dispose()
        {
            foreach(var item in RemoteShares)
            {
                item.Dispose();
            }
        }

        public IEnumerator<RemoteShare> GetEnumerator()
        {
            return ((IEnumerable<RemoteShare>)RemoteShares).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return RemoteShares.GetEnumerator();
        }
    }
}