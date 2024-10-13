using LanCloud.Shared.Dtos;
using LanCloud.Shared.Interfaces;
using LanCloud.Shared.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace LanCloud.Servers.Share
{
    public class LocalShareServerConnection : IDisposable
    {
        public LocalShareServerConnection(TcpClient client, ILocalShareHandler applicationHandler)
        {
            var RemoteEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
            Name = RemoteEndPoint.Address.ToString();

            Client = client;
            ShareHandler = applicationHandler;
        }

        public string Name { get; }
        public TcpClient Client { get; }
        public ILocalShareHandler ShareHandler { get; }

        public void HandleClient(object state)
        {
            using (var stream = Client.GetStream())
            using (var reader = new BinaryReader(stream))
            using (var writer = new BinaryWriter(stream))
            {
                while (Client.Connected)
                {
                    var requestJson = reader.ReadString();
                    var requestDataLength = reader.ReadInt32();
                    byte[] requestData = null;
                    if (requestDataLength >= 0)
                    {
                        requestData = reader.ReadBytes(requestDataLength);
                    }
                    var request = new LocalShareRequest(requestJson, requestData);
                    var response = ShareHandler.Receive(request);
                    writer.Write(response.Json);
                    writer.Write(Convert.ToInt32(response.Data?.Length ?? -1));
                    if (requestDataLength >= 0)
                    {
                        writer.Write(response.Data);
                    }
                }
            }
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}