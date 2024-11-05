using LanCloud.Shared.Log;
using LanCloud.Servers.Wjp;
using Newtonsoft.Json;
using System.Linq;
using LanCloud.Enums;
using System;
using LanCloud.Models.Responses;
using LanCloud.Models.Dtos;
using LanCloud.Domain.Collections;

namespace LanCloud.Domain.Application
{
    public class LocalApplicationHandler : IWjpHandler
    {
        public LocalApplicationHandler(
            LocalApplication application, 
            LocalShareCollection shares, 
            ILogger logger)
        {
            Application = application;
            Shares = shares;
            Logger = logger;

            //Logger.Info($"Loaded");
        }

        public LocalApplication Application { get; }
        private LocalShareCollection Shares { get; }
        public ILogger Logger { get; }

        public WjpResponse ProcessRequest(WjpRequest request)
        {
            switch (request.MessageType)
            {
                case (int)ApplicationMessageEnum.Ping:
                    return Handle_Ping();
                case (int)ApplicationMessageEnum.GetExternalShares:
                    return Handle_GetExternalShareDtos();
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
        private WjpResponse Handle_GetExternalShareDtos()
        {
            var list = Shares
                .Select(a => new ShareDto()
                {
                    Port = a.Port,
                    FreeSpace = a.GetFreeSpace()
                })
                .ToArray();
            var json = JsonConvert.SerializeObject(list);
            return new WjpResponse(json, null);
        }
    }
}