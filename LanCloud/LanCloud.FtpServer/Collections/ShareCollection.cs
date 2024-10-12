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
    internal class ShareCollection : IEnumerable<Share>, IDisposable
    {
        public ShareCollection(ApplicationConfig applicationConfig)
        {
            var port = applicationConfig.Port;
            Shares = applicationConfig.Shares
                .Select(shareConfig => new Share(IPAddress.Any, ++port, shareConfig))
                .ToArray();
        }

        public Share[] Shares { get; }

        public IEnumerator<Share> GetEnumerator()
        {
            return ((IEnumerable<Share>)Shares).GetEnumerator();
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