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
using LanCloud.Domain.FileStripe;
using System.Collections.Generic;

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

            if (!Root.Exists) Root.Create();

            FileStripeInfos = Root
                .GetFiles($"*.filestripe")
                .Select(fileRefInfo => new LocalFileStripe(fileRefInfo))
                .ToDictionary(a => a.Name);

            ShareStripes = config.Parts
                .Select(part => new LocalShareStripe(this, part, logger))
                .ToArray();

            if (Application.LocalApplicationServerConfig != null)
            {
                Server = new WjpServer(IPAddress.Any, Port, this, Application, Logger);
            }

            Status = Logger.Info($"OK");
        }

        public LocalApplication Application { get; }
        public LocalShareConfig Config { get; }
        public int Port { get; }
        public ILogger Logger { get; }
        private Dictionary<string, LocalFileStripe> FileStripeInfos { get; }
        public LocalShareStripe[] ShareStripes { get; }

        public string HostName => Application.LocalApplicationServerConfig?.HostName;
        public string RootFullName => Config.DirectoryName;
        public DirectoryInfo Root => new DirectoryInfo(RootFullName);

        public WjpServer Server { get; private set; }

        #region IShare interface
        IShareStripe[] IShare.ShareStripes => ShareStripes;
        IFileStripe IShare.FindFileStripe(string extention, string hash, long length, byte[] indexes)
        {
            return FindFileStripe(extention, hash, length, indexes);
        }
        #endregion

        public LocalFileStripe CreateTempFileStripe(string extention, byte[] indexes)
        {
            return new LocalFileStripe(Root, extention, indexes);
        }
        public void AddFileStripe(LocalFileStripe fileStripe)
        {
            lock (FileStripeInfos)
            {
                FileStripeInfos.Add(fileStripe.Name, fileStripe);
            }
        }
        public void RemoveFileStripe(LocalFileStripe fileStripe)
        {
            lock (FileStripeInfos)
            {
                FileStripeInfos.Remove(fileStripe.Name);
            }
        }
        public LocalFileStripe FindFileStripe(string extention, string hash, long length, byte[] indexes)
        {
            lock (FileStripeInfos)
            {
                var name = LocalFileStripe.CreateFileName(extention, hash, length, indexes);
                if (FileStripeInfos.TryGetValue(name, out var file)) return file;
                return null;
            }
        }

        public void ProcessRequest(int requestMessageType, string requestJson, byte[] requestData, int requestDataLength, out string responseJson, byte[] responseData, out int responseDataLength)
        {
            switch (requestMessageType)
            {
                case (int)ShareMessageEnum.FindFileStripes:
                    Handle_FindFileStripe(requestJson, out responseJson, responseData, out responseDataLength);
                    break;
                case (int)ShareMessageEnum.CreateFileStripeSession:
                    Handle_CreateFileStripeSession(requestJson, out responseJson, responseData, out responseDataLength);
                    break;
                case (int)ShareMessageEnum.StoreFileStripePart:
                    Handle_StoreFileStripeChunk(requestJson, out responseJson, responseData, out responseDataLength);
                    break;
                case (int)ShareMessageEnum.CloseFileStripeSession:
                    Handle_CloseFileStripeSession(requestJson, out responseJson, responseData, out responseDataLength);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void Handle_FindFileStripe(string requestJson, out string responseJson, byte[] responseData, out int responseDataLength)
        {
            var request = JsonConvert.DeserializeObject<FindFileStripesRequest>(requestJson);
            var localFileStripe = FindFileStripe(request.Extention, request.Hash, request.Length, request.Indexes);
            var remoteFileStripe = new FileStripeDto(localFileStripe);
            responseJson = JsonConvert.SerializeObject(remoteFileStripe);
            responseDataLength = 0;
        }

        private void Handle_CreateFileStripeSession(string requestJson, out string responseJson, byte[] responseData, out int responseDataLength)
        {
            throw new NotImplementedException();
        }
        private void Handle_StoreFileStripeChunk(string requestJson, out string responseJson, byte[] responseData, out int responseDataLength)
        {
            StoreFileStripeChunkRequest wjpRequest = JsonConvert.DeserializeObject<StoreFileStripeChunkRequest>(requestJson);
            responseJson = null;
            responseDataLength = 0;
        }
        private void Handle_CloseFileStripeSession(string requestJson, out string responseJson, byte[] responseData, out int responseDataLength)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Server.Dispose();
        }

    }
}