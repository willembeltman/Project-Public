using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace LanCloud.Servers.Wjp
{
    public class WjpClientConnection : IDisposable
    {
        public WjpClientConnection(WjpServer localShareServer, TcpClient client, IWjpHandler applicationHandler)
        {
            ShareServer = localShareServer;
            var RemoteEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
            Name = RemoteEndPoint.Address.ToString();
            Client = client;
            ShareHandler = applicationHandler;

            ShareServer.AddConnection(this);
        }

        public string Name { get; }
        public TcpClient Client { get; }
        public IWjpHandler ShareHandler { get; }
        public WjpServer ShareServer { get; }
        public Thread Thread { get; private set; }

        public void Start()
        {
            Thread = new Thread(new ThreadStart(HandleClient));
            Thread.Start();
        }
        public void HandleClient()
        {
            using (var stream = Client.GetStream())
            using (var reader = new BinaryReader(stream))
            using (var writer = new BinaryWriter(stream))
            {
                var requestMessageType = 0;
                var requestJson = string.Empty;
                var requestDataLength = 0;
                byte[] requestData = null;
                try
                {
                    while (Client.Connected)
                    {
                        requestMessageType = reader.ReadInt32();
                        if (requestMessageType == -1)
                        {
                            writer.Write(-1);
                        }
                        else
                        {
                            requestJson = reader.ReadString();
                            requestDataLength = reader.ReadInt32();
                            if (requestDataLength >= 0)
                            {
                                requestData = reader.ReadBytes(requestDataLength);
                            }
                            var request = new WjpRequest(requestMessageType, requestJson, requestData);
                            var response = ShareHandler.ProcessRequest(request);
                            writer.Write(response.Json);
                            writer.Write(Convert.ToInt32(response.Data?.Length ?? -1));
                            if (requestDataLength >= 0)
                            {
                                writer.Write(response.Data);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            Dispose();
        }

        public void Dispose()
        {
            Client.Dispose();
            ShareServer.RemoveConnection(this);
        }
    }
}