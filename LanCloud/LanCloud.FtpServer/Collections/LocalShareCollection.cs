using LanCloud.Servers.Ftp;
using LanCloud.Servers.Ftp.Interfaces;
using LanCloud.Handlers;
using LanCloud.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;

namespace LanCloud
{
    public class LocalShareCollection : IEnumerable<LocalShare>, IDisposable
    {
        public LocalShareCollection(Config applicationConfig)
        {
            var port = applicationConfig.Port;
            Shares = applicationConfig.Shares
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