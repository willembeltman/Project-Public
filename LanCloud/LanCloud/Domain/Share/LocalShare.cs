using LanCloud.Shared.Log;
using LanCloud.Models.Configs;
using LanCloud.Domain.Application;
using System;
using System.IO;
using LanCloud.Enums;
using LanCloud.Models.Share.Requests;
using Newtonsoft.Json;
using LanCloud.Servers.Wjp;
using System.Net;
using System.Linq;
using LanCloud.Models.Dtos;

namespace LanCloud.Domain.Share
{
    public class LocalShare : IWjpHandler, IDisposable, IShare
    {
        private string _Status { get; set; }
        public string Status
        {
            get => _Status;
            set
            {
                _Status = value;
                Application.StatusChanged();
            }
        }

        public LocalShare(LocalApplication application, LocalShareConfig config, int port, ILogger logger)
        {
            Application = application;
            Config = config;
            Port = port;
            Logger = logger;

            ShareStripes = config.Parts
                .Select(part => new LocalShareStripe(this, part, logger))
                .ToArray();
            FileBits = new FileStripeCollection(this, Logger);

            if (Application.ApplicationServerConfig != null)
            {
                Server = new WjpServer(IPAddress.Any, Port, this, Application, Logger);
            }

            Status = Logger.Info($"OK");
        }

        public LocalApplication Application { get; }
        public LocalShareConfig Config { get; }
        public int Port { get; }
        public ILogger Logger { get; }

        public LocalShareStripe[] ShareStripes { get; }
        public FileStripeCollection FileBits { get; }

        public string HostName => Application.ApplicationServerConfig?.HostName;
        public string RootFullName => Config.DirectoryName;
        public DirectoryInfo Root => new DirectoryInfo(RootFullName);

        public WjpServer Server { get; private set; }

        IShareStripe[] IShare.ShareStripes => ShareStripes;


        public FileStripeDto[] ListFileBits()
        {
            return FileBits.List()
                .Select(fileStripe => new FileStripeDto(fileStripe))
                .ToArray();
        }

        public void ProcessRequest(int requestMessageType, string requestJson, byte[] requestData, int requestDataLength, out string responseJson, byte[] responseData, out int responseDataLength)
        {
            switch (requestMessageType)
            {
                case (int)ShareMessageEnum.ListFileBits:
                    Handle_ListFileBits(requestJson, out responseJson, responseData, out responseDataLength);
                    break;
                case (int)ShareMessageEnum.CreateFileBitSession:
                    Handle_CreateFileBitSession(requestJson, out responseJson, responseData, out responseDataLength);
                    break;
                case (int)ShareMessageEnum.StoreFileBitPart:
                    Handle_StoreFileBitPart(requestJson, out responseJson, responseData, out responseDataLength);
                    break;
                case (int)ShareMessageEnum.CloseFileBitSession:
                    Handle_CloseFileBitSession(requestJson, out responseJson, responseData, out responseDataLength);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void Handle_ListFileBits(string requestJson, out string responseJson, byte[] responseData, out int responseDataLength)
        {
            responseJson = JsonConvert.SerializeObject(ListFileBits());
            responseDataLength = 0;
        }

        private void Handle_CloseFileBitSession(string requestJson, out string responseJson, byte[] responseData, out int responseDataLength)
        {
            throw new NotImplementedException();
        }

        private void Handle_CreateFileBitSession(string requestJson, out string responseJson, byte[] responseData, out int responseDataLength)
        {
            throw new NotImplementedException();
        }

        private void Handle_StoreFileBitPart(string requestJson, out string responseJson, byte[] responseData, out int responseDataLength)
        {
            StoreFileBitPartRequest wjpRequest = JsonConvert.DeserializeObject<StoreFileBitPartRequest>(requestJson);
            responseJson = null;
            responseDataLength = 0;
        }

        public void Dispose()
        {
            Server.Dispose();
        }

    }
}