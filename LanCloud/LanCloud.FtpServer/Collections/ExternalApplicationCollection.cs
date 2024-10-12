using LanCloud.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LanCloud
{
    internal class ExternalApplicationCollection : IEnumerable<ExternalApplication>, IDisposable
    {
        public ExternalApplicationCollection(ApplicationConfig config)
        {
            ExternalApplications = config.Servers
                .Where(a => a.IsThisComputer == false)
                .Select(serverName => new ExternalApplication(serverName))
                .ToArray();
        }

        public ExternalApplication[] ExternalApplications { get; }

        public void Dispose()
        {
            foreach (var externalApplication in ExternalApplications)
            {
                externalApplication.Dispose();
            };
        }

        public IEnumerator<ExternalApplication> GetEnumerator()
        {
            return ((IEnumerable<ExternalApplication>)ExternalApplications).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ExternalApplications.GetEnumerator();
        }
    }
}