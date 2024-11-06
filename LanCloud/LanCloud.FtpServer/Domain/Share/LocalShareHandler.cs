using LanCloud.Shared.Log;
using LanCloud.Servers.Wjp;
using Newtonsoft.Json;
using LanCloud.Enums;
using LanCloud.Models.Share.Responses;
using System;

namespace LanCloud.Domain.Share
{
    public class LocalShareHandler : IWjpHandler
    {
        public LocalShareHandler(LocalSharePart localShare, ILogger logger)
        {
            LocalShare = localShare;
            Logger = logger;

            Logger.Info($"Loaded");
        }

        public LocalSharePart LocalShare { get; }
        public ILogger Logger { get; }

        public WjpResponse ProcessRequest(WjpRequest request)
        {
            switch (request.MessageType)
            {
                case (int)ShareMessageEnum.Ping:
                    return Handle_Ping();
                default:
                    throw new NotImplementedException();
            }
        }

        private WjpResponse Handle_Ping()
        {
            var model = new PingResponse()
            {
                Pong = true
            };
            var json = JsonConvert.SerializeObject(model);
            return new WjpResponse(json, null);
        }
    }
}