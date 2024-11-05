using LanCloud.Domain.Application;
using LanCloud.Models.Configs;
using LanCloud.Shared.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LanCloud.Domain.Collections
{
    public class RemoteApplicationCollection : IEnumerable<RemoteApplication>, IDisposable
    {
        public RemoteApplicationCollection(LocalApplication localApplication, ILogger logger)
        {
            LocalApplication = localApplication;
            Logger = logger;

            RemoteApplications = Config.Servers
                .Where(a => a.IsThisComputer == false)
                .Select(remoteconfig => new RemoteApplication(remoteconfig, logger))
                .ToArray();

            //Logger.Info("Loaded");
        }

        public LocalApplication LocalApplication { get; }
        public ILogger Logger { get; }

        public RemoteApplication[] RemoteApplications { get; }

        public ApplicationConfig Config => LocalApplication.Config;

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