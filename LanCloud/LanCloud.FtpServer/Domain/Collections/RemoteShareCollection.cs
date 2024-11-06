using LanCloud.Domain.Application;
using LanCloud.Domain.Share;
using LanCloud.Shared.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LanCloud.Domain.Collections
{
    public class RemoteShareCollection : IEnumerable<RemoteShare>
    {
        public RemoteShareCollection(LocalApplication application, ILogger logger)
        {
            Application = application;
            Logger = logger;
        }

        public LocalApplication Application { get; }
        public ILogger Logger { get; }

        public RemoteShare[] RemoteShares => 
            Application.RemoteApplications
                .Where(a => a.Connected)
                .SelectMany(a => a.RemoteShares)
                .ToArray();

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