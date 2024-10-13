using LanCloud.Models;
using LanCloud.Shared.Dtos;
using LanCloud.Shared.Interfaces;
using LanCloud.Shared.Messages;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace LanCloud.Handlers
{
    public class LocalApplicationHandler : ILocalApplicationHandler
    {
        public LocalApplicationHandler(LocalApplication application, LocalShareCollection shares)
        {
            Application = application;
            Shares = shares;
        }

        public LocalApplication Application { get; }
        private LocalShareCollection Shares { get; }

        public string Receive(ApplicationMessages message)
        {
            switch (message) {
                case ApplicationMessages.Ping:
                    return Handle_Ping();
                case ApplicationMessages.GetExternalShareDtos:
                    return Handle_GetExternalShareDtos();
                default:
                    throw new NotImplementedException();
            }
        }

        private string Handle_Ping()
        {
            return "Pong";
        }
        private string Handle_GetExternalShareDtos()
        {
            var list = Shares
                .Select(a => new ShareDto()
                {
                    Port = a.Port,
                    FreeSpace = a.GetFreeSpace()
                })
                .ToArray();
            var json = JsonConvert.SerializeObject(list);
            return json;
        }

    }
}