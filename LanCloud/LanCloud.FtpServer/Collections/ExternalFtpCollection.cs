using LanCloud.Models;
using System;

namespace LanCloud
{
    internal class ExternalFtpCollection : IDisposable
    {
        public ExternalFtpCollection(ExternalApplicationCollection externalApplications)
        {
            ExternalApplications = externalApplications;
        }

        public ExternalApplicationCollection ExternalApplications { get; }

        public void Dispose()
        {
            ExternalApplications.Dispose();
        }


    }
}