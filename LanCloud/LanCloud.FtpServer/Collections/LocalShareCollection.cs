using LanCloud.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using LanCloud.Shared.Log;
using LanCloud.Configs;
using LanCloud.Domain.Share;

namespace LanCloud.Collections
{
    public class LocalShareCollection : IEnumerable<LocalShare>, IDisposable
    {
        public LocalShareCollection(Config config, ILogger logger)
        {
            var port = config.StartPort;
            Shares = config.Shares
                .Select(shareConfig => 
                    new LocalShare(IPAddress.Any, ++port, shareConfig, logger))
                .ToArray();
            Logger = logger;
        }

        public LocalShare[] Shares { get; }
        public ILogger Logger { get; }

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