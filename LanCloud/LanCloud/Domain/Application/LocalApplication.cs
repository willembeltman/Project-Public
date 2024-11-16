using LanCloud.Domain.Share;
using LanCloud.Enums;
using LanCloud.Models;
using LanCloud.Models.Configs;
using LanCloud.Models.Dtos;
using LanCloud.Servers.Ftp;
using LanCloud.Servers.Wjp;
using LanCloud.Shared.Log;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;

namespace LanCloud.Domain.Application
{
    public class LocalApplication : IDisposable, IWjpApplication, IFtpApplication, IWjpHandler
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
                .Select(share => new LocalShare(this, share, ++port, Logger))
                .ToArray();

            RemoteApplications = Config.Servers
                .Where(a => a.IsThisComputer == false)
                .Select(remoteconfig => new RemoteApplication(this, remoteconfig, logger))
                .ToArray();

            if (ApplicationServerConfig != null)
            {
                Server = new WjpServer(IPAddress.Any, ApplicationServerConfig.Port, this, this, logger);
            }

            VirtualFtpServer = new VirtualFtp.VirtualFtp(this, logger);

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
        public WjpServer Server { get; }
        public VirtualFtp.VirtualFtp VirtualFtpServer { get; }
        private string _Status { get; set; }

        public string HostName => Config.HostName;
        public int FileBitBufferSize => Config.FileBitBufferSize;
        public int WjpBufferSize => Config.WjpBufferSize;
        public int FtpBufferSize => Config.FtpBufferSize;
        public int? Port => ApplicationServerConfig?.Port;
        public LocalShareStripe[] LocalShareBits => LocalShares?
            .SelectMany(a => a.ShareStripes)
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

        public FileStripe[] FindFileBits(string extention, FileRef fileRef, FileRefStripe fileRefBit)
        {
            var fileBits = LocalShares
                .Select(a => a.FileBits.FindFileBit(extention, fileRef, fileRefBit))
                .Where(a => a != null)
                .ToArray();
            return fileBits;
        }

        public void ProcessRequest(
            int requestMessageType, string requestJson, byte[] requestData, int requestDataLength,
            out string responseJson, byte[] responseData, out int responseDataLength)
        {
            switch (requestMessageType)
            {
                case (int)ApplicationMessageEnum.GetExternalShares:
                    Handle_GetExternalShareDtos(out responseJson, out responseDataLength);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void Handle_GetExternalShareDtos(out string responseJson, out int responseDataLength)
        {
            var shareDtos = LocalShares
                .Select(a => new ShareDto()
                {
                    HostName = a.HostName,
                    Port = a.Port
                })
                .ToArray();
            responseJson = JsonConvert.SerializeObject(shareDtos);
            responseDataLength = 0;
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