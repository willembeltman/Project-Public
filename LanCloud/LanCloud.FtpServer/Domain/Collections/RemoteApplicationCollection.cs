using LanCloud.Domain.Application;
using LanCloud.Models.Configs;
using LanCloud.Shared.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LanCloud.Collections
{
    public class RemoteApplicationCollection : IEnumerable<RemoteApplication>, IDisposable
    {
        public RemoteApplicationCollection(ApplicationConfig config, ILogger logger)
        {
            Config = config;
            Logger = logger;

            RemoteApplications = config.Servers
                .Where(a => a.IsThisComputer == false)
                .Select(remoteconfig => new RemoteApplication(remoteconfig, logger))
                .ToArray();

            //Logger.Info("Loaded");
        }

        public ApplicationConfig Config { get; }
        public ILogger Logger { get; }
        public RemoteApplication[] RemoteApplications { get; }

        public void Dispose()
        {
            foreach (var externalApplication in RemoteApplications)
            {
                externalApplication.Dispose();
            };
        }

        public IEnumerator<RemoteApplication> GetEnumerator()
        {
            return ((IEnumerable<RemoteApplication>)RemoteApplications).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return RemoteApplications.GetEnumerator();
        }
    }
}