using LanCloud.Domain.Application;
using LanCloud.Domain.Share;
using LanCloud.Shared.Log;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LanCloud.Domain.Collections
{
    public class RemoteShareCollection : IEnumerable<RemoteShare>, IDisposable
    {
        public RemoteShareCollection(LocalApplication application, ILogger logger)
        {
            Application = application;
            Logger = logger;

            RemoteShares = new RemoteShare[0];

            //Logger.Info($"Loaded");
        }

        public LocalApplication Application { get; }
        public ILogger Logger { get; }

        public RemoteApplicationCollection RemoteApplications => Application.RemoteApplications;
        public RemoteShare[] RemoteShares { get; }

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