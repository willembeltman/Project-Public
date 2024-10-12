using LanCloud.Handlers;
using LanCloud.Ftp;
using System;
using System.Net;

namespace LanCloud.Models
{
    internal class VirtualFtp : IDisposable
    {
        public VirtualFtp(
            FolderCollection ownFtpShares, 
            ExternalApplicationCollection externalApplications, 
            ExternalFtpCollection externalFtpShares)
        {
            FtpHandler = new VirtualFtpHandler(ownFtpShares, externalApplications, externalFtpShares);
            FtpServer = new Server(IPAddress.Any, 21, FtpHandler);
        }

        public VirtualFtpHandler FtpHandler { get; }
        public Server FtpServer { get; }

        public void Dispose()
        {
            FtpServer.Dispose();
        }
    }
}