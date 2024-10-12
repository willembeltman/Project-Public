using LanCloud.Servers.Application.Interfaces;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace LanCloud.Servers.Application
{
    internal class ApplicationServerClient : IDisposable
    {
        public ApplicationServerClient(TcpClient client, IApplicationHandler applicationHandler)
        {
            var RemoteEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
            Name = RemoteEndPoint.Address.ToString();

            Client = client;
            ApplicationHandler = applicationHandler;
        }

        public string Name { get; }
        public TcpClient Client { get; }
        public IApplicationHandler ApplicationHandler { get; }
        public NetworkStream Stream { get; private set; }
        public BinaryReader Reader { get; private set; }
        public BinaryWriter Writer { get; private set; }

        public void HandleClient(object state)
        {
            using (Stream = Client.GetStream())
            using (Reader = new BinaryReader(Stream))
            using (Writer = new BinaryWriter(Stream))
            {
                string request;
                while ((request = Reader.ReadString()) != null)
                {
                    var response = ApplicationHandler.Receive(request);
                    Writer.Write(response);
                }
            }
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}