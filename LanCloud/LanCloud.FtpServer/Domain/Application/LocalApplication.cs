using LanCloud.Collections;
using LanCloud.Domain.IO;
using LanCloud.Models.Configs;
using LanCloud.Servers.Wjp;
using LanCloud.Shared.Log;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace LanCloud.Domain.Application
{
    public class LocalApplication : IDisposable
    {
        public LocalApplication(
            ApplicationConfig config,
            RemoteApplicationCollection remoteApplications,
            RemoteShareCollection remoteShares,
            ILogger logger)
        {
            Config = config;
            RemoteApplications = remoteApplications;
            RemoteShares = remoteShares;
            Logger = logger;

            var rootInfo = new DirectoryInfo(config.FileDatabaseDirectoryName);
            if (!rootInfo.Exists) { rootInfo.Create(); }
            RootDirectory = rootInfo.FullName.TrimEnd('\\');

            ServerConfig = config.Servers.FirstOrDefault(a => a.IsThisComputer);
            if (ServerConfig != null)
            {
                LocalShares = new LocalShareCollection(this, logger);
                Authentication = new AuthenticationService(this, logger);
                ServerHandler = new LocalApplicationHandler(this, LocalShares, logger);
                Server = new WjpServer(IPAddress.Any, config.StartPort, ServerHandler, logger);
                Logger.Info($"Loaded");
            }
            else
            {
                Logger.Info($"Loaded without server");
            }
        }

        public ApplicationConfig Config { get; }
        public RemoteApplicationConfig ServerConfig { get; }
        public LocalShareCollection LocalShares { get; }
        public RemoteApplicationCollection RemoteApplications { get; }
        public RemoteShareCollection RemoteShares { get; }
        public ILogger Logger { get; }
        public string RootDirectory { get; }
        public AuthenticationService Authentication { get; }
        public LocalApplicationHandler ServerHandler { get; }
        public WjpServer Server { get; }
        
        public string HostName => ServerConfig.HostName;

        public FileBit[] FindFileBits(FileRef fileRef, FileRefBit fileRefBit)
        {
            var fileBits = LocalShares
                .Select(a => a.Storage.FindFileBit(fileRef, fileRefBit))
                .Where(a => a != null)
                .ToArray();
            return fileBits;
        }

        public void Dispose()
        {
            Server.Dispose();
            LocalShares.Dispose();
        }

    }
}