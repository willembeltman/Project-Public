using LanCloud.Domain.Application;
using LanCloud.Domain.Share;
using LanCloud.Models.Configs;
using LanCloud.Shared.Log;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LanCloud.Collections
{
    public class RemoteShareCollection : IEnumerable<RemoteShare>, IDisposable
    {
        public RemoteShareCollection(RemoteApplicationCollection remoteApplications, ILogger logger)
        {
            RemoteApplications = remoteApplications;
            Logger = logger;

            RemoteShares = new RemoteShare[0];

            Logger.Info($"Loaded");
        }

        public RemoteApplicationCollection RemoteApplications { get; }
        public RemoteShare[] RemoteShares { get; }
        public ILogger Logger { get; }

        public void Dispose()
        {
            RemoteApplications.Dispose();
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