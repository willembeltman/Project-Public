using LanCloud.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using LanCloud.Shared.Models;

namespace LanCloud
{
    public class LocalShareCollection : IEnumerable<LocalShare>, IDisposable
    {
        public LocalShareCollection(Config config)
        {
            var port = config.Port;
            Shares = config.Shares
                .Select(shareConfig => new LocalShare(IPAddress.Any, ++port, shareConfig))
                .ToArray();
        }

        public LocalShare[] Shares { get; }

        public IEnumerator<LocalShare> GetEnumerator()
        {
            return ((IEnumerable<LocalShare>)Shares).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Shares.GetEnumerator();
        }

        public void Dispose()
        {
            foreach (var item in Shares)
                item.Dispose();
        }
    }
}