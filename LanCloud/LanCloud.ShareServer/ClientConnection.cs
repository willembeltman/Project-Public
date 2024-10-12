using LanCloud.Servers.Share.Interfaces;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace LanCloud.Servers.Share
{
    internal class ClientConnection : IDisposable
    {
        public ClientConnection(TcpClient client, IShareHandler applicationHandler)
        {
            var RemoteEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
            Name = RemoteEndPoint.Address.ToString();

            Client = client;
            ShareHandler = applicationHandler;
        }

        public string Name { get; }
        public TcpClient Client { get; }
        public IShareHandler ShareHandler { get; }

        public void HandleClient(object state)
        {
            using (var stream = Client.GetStream())
            using (var reader = new BinaryReader(stream))
            using (var writer = new BinaryWriter(stream))
            {
                string requestJson;
                while ((requestJson = reader.ReadString()) != null)
                {
                    var requestDto = JsonConvert.DeserializeObject<ShareRequestDto>(requestJson);
                    var request = new ShareRequest(requestDto, stream);
                    var response = ShareHandler.Receive(request);
                    var responseJson = JsonConvert.SerializeObject(response);
                    writer.Write(responseJson);
                    if (response.Response.DataSize > 0)
                    {
                        response.DataStream.CopyTo(stream);
                    }
                }
            }
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
    public class ShareRequest
    {
        public ShareRequest(ShareRequestDto requestDto, Stream stream)
        {
            Request = requestDto;
            DataStream = stream;
        }

        public ShareRequestDto Request { get; set; }
        public Stream DataStream { get; set; }
    }
    public class ShareRequestDto
    {
        public string Method { get; set; }
        public string[] Arguments { get; set; }
        public long DataSize { get; set; }
    }
    public class ShareResponse
    {
        public ShareResponse(ShareResponseDto responseDto, Stream stream)
        {
            Response = responseDto;
            DataStream = stream;
        }
        public ShareResponseDto Response { get; set; }
        public Stream DataStream { get; set; }
    }
    public class ShareResponseDto
    {
        public string Arguments { get; set; }
        public long DataSize { get; set; }
    }
}