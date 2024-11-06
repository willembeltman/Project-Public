using LanCloud.Shared.Log;
using LanCloud.Servers.Wjp;
using System;
using System.Net;
using LanCloud.Models.Configs;
using LanCloud.Domain.Application;
using System.Collections.Generic;
using LanCloud.Domain.VirtualFtp;
using LanCloud.Domain.IO;

namespace LanCloud.Domain.Share
{
    public class LocalSharePart : IDisposable, IShare
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

        public LocalSharePart(LocalShare localShare, LocalSharePartConfig partConfig, string hostName, int port, ILogger logger)
        {
            LocalShare = localShare;
            HostName = hostName;
            Port = port;
            PartConfig = partConfig;
            Logger = logger;

            if (localShare.Application.RemoteApplicationConfig != null)
            {
                ServerHandler = new LocalShareHandler(this, Logger);
                Server = new WjpServer(IPAddress.Any, port, ServerHandler, Application, Logger);
            }

            Status = Logger.Info($"OK");
        }

        public LocalShare LocalShare { get; }
        public LocalSharePartConfig PartConfig { get; }
        public string HostName { get; }
        public int Port { get; }
        public ILogger Logger { get; }

        public LocalShareHandler ServerHandler { get; }
        public WjpServer Server { get; }

        public LocalApplication Application => LocalShare.Application;
        public int[] Indexes => PartConfig.Indexes;


        public bool SaveFile(FileBit bit, IEnumerable<SingleBuffer> datareader)
        {
            if (datareader == null) return false;

            using (var stream = bit.OpenWrite())
            {
                foreach (var data in datareader)
                {
                    stream.Write(data.Buffer, 0, data.BufferPosition);
                }
            }

            return true;
        }
        public IEnumerable<SingleBuffer> LoadFile(string path)
        {
            var fileinfo = new PathInfo(Application, path, Logger);
            if (!fileinfo.Exists) return null;
            return LoadFileYield(fileinfo);
        }

        private IEnumerable<SingleBuffer> LoadFileYield(PathInfo fileinfo)
        {
            SingleBuffer buffer = new SingleBuffer();
            using (var stream = fileinfo.OpenRead())
            {
                while ((buffer.BufferPosition = stream.Read(buffer.Buffer, 0, buffer.Length)) > 0)
                {
                    yield return buffer;
                }
            }
        }

        public void Dispose()
        {
            Server?.Dispose();
        }
    }
}