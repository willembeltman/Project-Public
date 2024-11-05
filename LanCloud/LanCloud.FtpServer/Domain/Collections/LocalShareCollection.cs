using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LanCloud.Shared.Log;
using LanCloud.Domain.Share;
using LanCloud.Domain.Application;

namespace LanCloud.Collections
{
    public class LocalShareCollection : IEnumerable<LocalShare>, IDisposable
    {
        public LocalShareCollection(LocalApplication application, ILogger logger)
        {
            Application = application;
            Logger = logger;

            var config = application.Config;
            var port = config.StartPort;
            Shares = config.Shares
                .Select(shareConfig => new LocalShare(this, ++port, config, shareConfig, logger))
                .ToArray();


            //Logger.Info("Loaded");
        }

        public LocalApplication Application { get; }
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