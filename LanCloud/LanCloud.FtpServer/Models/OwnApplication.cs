using LanCloud.Handlers;
using LanCloud.ApplicationServer;
using System;
using System.Net;

namespace LanCloud.Models
{
    internal class OwnApplication : IDisposable
    {
        public OwnApplication(
            FolderCollection ownFtpShares, 
            ExternalApplicationCollection externalApplications, 
            ExternalFtpCollection externalFtpShares)
        {
            ApplicationHandler = new OwnApplicationHandler(ownFtpShares, externalApplications, externalFtpShares);
            ApplicationServer = new Server(IPAddress.Any, 21, ApplicationHandler);
        }

        public OwnApplicationHandler ApplicationHandler { get; }
        public Server ApplicationServer { get; }

        public void Dispose()
        {
            ApplicationServer.Dispose();
        }
    }
}