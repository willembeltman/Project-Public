using LanCloud.Domain.Application;
using LanCloud.Servers.Ftp;
using LanCloud.Shared.Log;
using System;
using System.Net;

namespace LanCloud.Domain.VirtualFtp
{
    public class LocalVirtualFtp : IDisposable
    {
        public LocalVirtualFtp(LocalApplication application, ILogger logger)
        {
            FtpHandler = new LocalVirtualFtpHandler(application, logger);
            FtpServer = new FtpServer(IPAddress.Any, 21, FtpHandler, logger);
        }

        public LocalVirtualFtpHandler FtpHandler { get; }
        public FtpServer FtpServer { get; }

        public void Dispose()
        {
            FtpServer.Dispose();
        }
    }
}