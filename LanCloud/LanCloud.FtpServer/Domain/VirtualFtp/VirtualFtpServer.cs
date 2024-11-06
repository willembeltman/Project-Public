using LanCloud.Domain.Application;
using LanCloud.Servers.Ftp;
using LanCloud.Shared.Log;
using System;
using System.Net;

namespace LanCloud.Domain.VirtualFtp
{
    public class VirtualFtpServer : IDisposable
    {
        public VirtualFtpServer(LocalApplication application, ILogger logger)
        {
            Application = application;
            Logger = logger;

            FtpHandler = new VirtualFtpHandler(application, logger);
            FtpServer = new FtpServer(IPAddress.Any, 21, FtpHandler, application, logger);

            //Logger.Info($"Loaded");
        }

        public LocalApplication Application { get; }
        public ILogger Logger { get; }
        public VirtualFtpHandler FtpHandler { get; }
        public FtpServer FtpServer { get; }

        public void Dispose()
        {
            FtpServer.Dispose();
        }
    }
}