using LanCloud.Handlers;
using LanCloud.Servers.Ftp;
using System;
using System.Net;

namespace LanCloud.Models
{
    public class LocalVirtualFtp : IDisposable
    {
        public LocalVirtualFtp(LocalApplication application)
        {
            FtpHandler = new LocalVirtualFtpHandler(application);
            FtpServer = new FtpServer(IPAddress.Any, 21, FtpHandler);
        }

        public LocalVirtualFtpHandler FtpHandler { get; }
        public FtpServer FtpServer { get; }

        public void Dispose()
        {
            FtpServer.Dispose();
        }
    }
}