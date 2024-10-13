using LanCloud.Shared.Interfaces;
using LanCloud.Shared.Messages;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace LanCloud.Servers.Application
{
    public class LocalApplicationServerConnection : IDisposable
    {
        public LocalApplicationServerConnection(TcpClient client, ILocalApplicationHandler applicationHandler)
        {
            var RemoteEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
            RemoteAddress = RemoteEndPoint.Address.ToString();

            Client = client;
            ApplicationHandler = applicationHandler;
        }

        public string RemoteAddress { get; }
        public TcpClient Client { get; }
        public ILocalApplicationHandler ApplicationHandler { get; }

        public void HandleClient(object state)
        {
            using (var stream = Client.GetStream())
            using (var reader = new BinaryReader(stream))
            using (var writer = new BinaryWriter(stream))
            {
                try
                {
                    while (Client.Connected)
                    {
                        var request = reader.ReadInt32();
                        var message = (ApplicationMessages)request;
                        var response = ApplicationHandler.Receive(message);
                        writer.Write(response);
                    }
                }
                catch (EndOfStreamException ex)
                {

                }
            }
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}