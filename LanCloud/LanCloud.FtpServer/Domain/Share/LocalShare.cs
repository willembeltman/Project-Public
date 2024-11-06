using LanCloud.Shared.Log;
using LanCloud.Models.Configs;
using LanCloud.Domain.Application;
using System.Collections.Generic;
using System;
using System.IO;

namespace LanCloud.Domain.Share
{
    public class LocalShare : IDisposable
    {
        private string _Status { get; set; }
        public string Status
        {
            get => _Status;
            set
            {
                _Status = value;
                LocalApplication.StatusChanged();
            }
        }

        public LocalShare(LocalApplication application, LocalShareConfig shareConfig, ref int port, ILogger logger)
        {
            LocalApplication = application;
            LocalShareConfig = shareConfig;
            Logger = logger;

            var list = new List<LocalSharePart>();
            foreach (var part in shareConfig.Parts)
            {
                port = port + 1;
                list.Add(new LocalSharePart(this, part, port, logger));
            }
            LocalShareParts = list.ToArray();

            Status = Logger.Info($"OK");
        }

        public LocalApplication LocalApplication { get; }
        public LocalShareConfig LocalShareConfig { get; }
        public ILogger Logger { get; }

        public LocalSharePart[] LocalShareParts { get; }

        public string RootFullName => LocalShareConfig.DirectoryName;
        public DirectoryInfo Root => new DirectoryInfo(RootFullName);

        public void Dispose()
        {
            foreach (var part in LocalShareParts)
            {
                part.Dispose();
            }
        }
    }
}