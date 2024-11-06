using LanCloud.Domain.Collections;
using LanCloud.Domain.VirtualFtp;
using LanCloud.Models.Configs;
using LanCloud.Servers.Ftp;
using LanCloud.Servers.Wjp;
using LanCloud.Shared.Log;
using System;
using System.Linq;
using System.Net;

namespace LanCloud.Domain.Application
{
    public class LocalApplication : IDisposable, IWjpApplication, IFtpApplication
    {
        public event EventHandler OnStateChanged;
        private string _Status { get; set; }
        public string Status
        {
            get => _Status; 
            set
            {
                _Status = value; 
                StatusChanged();
            }
        }
        public void StatusChanged()
        {
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }

        public ApplicationConfig Config { get; }
        public ILogger Logger { get; }

        public AuthenticationService Authentication { get; }
        public FileRefCollection FileRefs { get; }
        public LocalShareCollection LocalShares { get; }
        public RemoteApplicationCollection RemoteApplications { get; }

        public RemoteApplicationConfig ServerConfig { get; }        
        public LocalApplicationHandler ServerHandler { get; }
        public WjpServer Server { get; }
        public VirtualFtpServer VirtualFtpServer { get; }

        public LocalApplication(
            ApplicationConfig config,
            ILogger logger)
        {
            Config = config;
            Logger = logger;

            Status = Logger.Info($"Constructing");

            Authentication = new AuthenticationService(this, logger);
            FileRefs = new FileRefCollection(this, logger);

            RemoteApplications = new RemoteApplicationCollection(this, logger);

            ServerConfig = config.Servers.FirstOrDefault(a => a.IsThisComputer);
            if (ServerConfig != null)
            {
                LocalShares = new LocalShareCollection(this, ServerConfig.HostName, logger);
                ServerHandler = new LocalApplicationHandler(this, ServerConfig.HostName, logger);
                Server = new WjpServer(IPAddress.Any, config.StartPort, ServerHandler, this, logger);

                Status = Logger.Info($"OK");
            }
            else
            {
                Status = Logger.Info($"OK without server");
            }

            VirtualFtpServer = new VirtualFtpServer(this, logger);
        }

        public string HostName => ServerConfig.HostName;
        public int Port => ServerConfig.Port;

        public void Dispose()
        {
            Server.Dispose();
            LocalShares.Dispose();
            RemoteApplications.Dispose();
        }

    }
}