using LanCloud.Domain.Share;
using LanCloud.Domain.VirtualFtp;
using LanCloud.Models;
using LanCloud.Models.Configs;
using LanCloud.Servers.Ftp;
using LanCloud.Servers.Wjp;
using LanCloud.Services;
using LanCloud.Shared.Log;
using System;
using System.Linq;
using System.Net;

namespace LanCloud.Domain.Application
{
    public class LocalApplication : IDisposable, IWjpApplication, IFtpApplication
    {
        public event EventHandler OnStateChanged;

        public LocalApplication(ApplicationConfig config, ILogger logger)
        {
            Config = config;
            Logger = logger;

            Status = Logger.Info($"Constructing");

            Authentication = new AuthenticationService(this, logger);
            FileRefs = new FileRefCollection(this, logger);

            ApplicationServerConfig = config.Servers.FirstOrDefault(a => a.IsThisComputer);
            
            int port = ApplicationServerConfig?.Port ?? 8080;
            LocalShares = Config.Shares
                .Select(share => new LocalShare(this, share, ref port, Logger))
                .ToArray();

            RemoteApplications = Config.Servers
                .Where(a => a.IsThisComputer == false)
                .Select(remoteconfig => new RemoteApplication(this, remoteconfig, logger))
                .ToArray();

            if (ApplicationServerConfig != null)
            {
                ServerHandler = new LocalApplicationHandler(this, ApplicationServerConfig.HostName, logger);
                Server = new WjpServer(IPAddress.Any, ApplicationServerConfig.Port, ServerHandler, this, logger);
            }

            VirtualFtpServer = new VirtualFtpServer(this, logger);

            if (ApplicationServerConfig != null)
            {
                Status = Logger.Info($"OK");
            }
            else
            {
                Status = Logger.Info($"OK without sharing");
            }
        }

        public ApplicationConfig Config { get; }
        public ILogger Logger { get; }

        public AuthenticationService Authentication { get; }
        public FileRefCollection FileRefs { get; }
        public RemoteApplication[] RemoteApplications { get; }
        public RemoteApplicationConfig ApplicationServerConfig { get; }
        public LocalShare[] LocalShares { get; }
        public LocalApplicationHandler ServerHandler { get; }
        public WjpServer Server { get; }
        public VirtualFtpServer VirtualFtpServer { get; }
        private string _Status { get; set; }

        public string HostName => Config.HostName;
        public int? Port => ApplicationServerConfig?.Port;
        public LocalSharePart[] LocalShareParts => LocalShares?
            .SelectMany(localShare => localShare.LocalShareParts)
            .ToArray();

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

        public FileBit[] FindFileBits(string extention, FileRef fileRef, FileRefBit fileRefBit)
        {
            var fileBits = LocalShareParts
                .Where(a => a.Indexes.Matches(fileRefBit.Indexes))
                .Select(a => a.FileBits.FindFileBit(extention, fileRef, fileRefBit))
                .Where(a => a != null)
                .ToArray();
            return fileBits;
        }

        public void Dispose()
        {
            Server?.Dispose();
            foreach (var item in RemoteApplications)
                item.Dispose();
            if (LocalShares != null)
            {
                foreach (var item in LocalShares)
                    item.Dispose();
            }
        }

    }
}