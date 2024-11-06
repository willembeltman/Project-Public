using LanCloud.Shared.Log;
using LanCloud.Models.Configs;
using LanCloud.Domain.Application;
using System.Collections.Generic;
using System;
using LanCloud.Domain.IO;

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
                Application.StatusChanged();
            }
        }

        public LocalShare(LocalApplication application, LocalShareConfig shareConfig, ref int port, ILogger logger)
        {
            Application = application;
            ShareConfig = shareConfig;
            Logger = logger;

            var list = new List<LocalSharePart>();
            foreach (var part in shareConfig.Parts)
            {
                port = port + 1;
                list.Add(new LocalSharePart(this, part, port, logger));
            }
            LocalShareParts = list.ToArray();

            FileBits = new FileBitCollection(this, Logger);

            Status = Logger.Info($"OK");
        }

        public LocalApplication Application { get; }
        public LocalShareConfig ShareConfig { get; }
        public ILogger Logger { get; }
        public LocalSharePart[] LocalShareParts { get; }
        public FileBitCollection FileBits { get; }


        public void Dispose()
        {
            foreach (var part in LocalShareParts)
            {
                part.Dispose();
            }
        }
    }
}