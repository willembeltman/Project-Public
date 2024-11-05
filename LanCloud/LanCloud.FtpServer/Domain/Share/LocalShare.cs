using LanCloud.Shared.Log;
using LanCloud.Servers.Wjp;
using System;
using System.Net;
using LanCloud.Models.Configs;
using System.Linq;
using LanCloud.Domain.Application;
using LanCloud.Domain.Collections;

namespace LanCloud.Domain.Share
{
    public class LocalShare : IDisposable, IShare
    {
        public LocalShare(LocalShareCollection localShareCollection, int port, ApplicationConfig config, LocalShareConfig shareConfig, ILogger logger)
        {
            LocalShareCollection = localShareCollection;
            Port = port;
            Config = shareConfig;
            Logger = logger;

            FileBits = new FileBitCollection(this, logger);
            ServerHandler = new LocalShareHandler(this, Logger);
            Parts = Config.Parts.Select(part => new LocalSharePart(this, part)).ToArray();
            Server = new WjpServer(IPAddress.Any, port, ServerHandler, Logger);

            //Logger.Info($"Loaded");
        }

        public LocalShareCollection LocalShareCollection { get; }
        public int Port { get; }
        public LocalShareConfig Config { get; }
        public ILogger Logger { get; }
        public FileBitCollection FileBits { get; }
        public LocalShareHandler ServerHandler { get; }
        public LocalSharePart[] Parts { get; }
        public WjpServer Server { get; }

        public void Dispose()
        {
            Server.Dispose();
        }

        public int GetFreeSpace()
        {
            return 0;
        }
    }
}