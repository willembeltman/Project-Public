using LanCloud.Shared.Log;
using LanCloud.Servers.Wjp;
using System;
using System.Net;
using LanCloud.Models.Configs;
using System.Linq;
using LanCloud.Domain.Collections;
using LanCloud.Domain.Application;

namespace LanCloud.Domain.Share
{
    public class LocalShare : IDisposable, IShare
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

        public LocalShare(LocalShareCollection localShareCollection, string hostName, int port, ApplicationConfig config, LocalShareConfig shareConfig, ILogger logger)
        {
            LocalShareCollection = localShareCollection;
            HostName = hostName;
            Port = port;
            Config = shareConfig;
            Logger = logger;

            FileBits = new FileBitCollection(this, Logger);
            Parts = new LocalSharePartCollection(this, Logger);
            ServerHandler = new LocalShareHandler(this, Logger);
            Server = new WjpServer(IPAddress.Any, port, ServerHandler, Logger);

            Status = Logger.Info($"OK");
        }

        public LocalShareCollection LocalShareCollection { get; }
        public string HostName { get; }
        public int Port { get; }
        public LocalShareConfig Config { get; }
        public ILogger Logger { get; }

        public FileBitCollection FileBits { get; }
        public LocalSharePartCollection Parts { get; }
        public LocalShareHandler ServerHandler { get; }
        public WjpServer Server { get; }

        public LocalApplication Application => LocalShareCollection.Application;

        public void Dispose()
        {
            Server.Dispose();
        }
    }
}