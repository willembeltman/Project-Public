using LanCloud.Handlers;
using LanCloud.Servers.Ftp;
using System;
using System.Net;

namespace LanCloud.Models
{
    internal class VirtualFtp : IDisposable
    {
        public VirtualFtp(Application application)
        {
            FtpHandler = new VirtualFtpHandler(application);
            FtpServer = new FtpServer(IPAddress.Any, 21, FtpHandler);
        }

        public VirtualFtpHandler FtpHandler { get; }
        public FtpServer FtpServer { get; }

        public void Dispose()
        {
            FtpServer.Dispose();
        }
    }
}