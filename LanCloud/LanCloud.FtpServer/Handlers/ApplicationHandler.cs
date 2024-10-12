using LanCloud.Servers.Application;
using LanCloud.Servers.Application.Interfaces;
using LanCloud.Dtos;
using LanCloud.Models;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace LanCloud.Handlers
{
    internal class ApplicationHandler : IApplicationHandler
    {
        private ShareCollection Shares;
        private ApplicationProxyCollection ExternalApplications;
        private ShareProxyCollection ExternalShares;

        public ApplicationHandler(
            ShareCollection shares, 
            ApplicationProxyCollection externalApplications, 
            ShareProxyCollection externalShares)
        {
            Shares = shares;
            ExternalApplications = externalApplications;
            ExternalShares = externalShares;
        }

        public string Receive(string request)
        {
            ApplicationProxy external = null;
            switch (request) {
                case nameof(external.Ping):
                    return Handle_Ping();
                case nameof(external.GetExternalShareDtos):
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
                .Select(a => new ExternalShareDto()
                {
                    IpAdress = a.IpAdress,
                    Port = a.Port,
                    FreeSpace = a.GetFreeSpace()
                })
                .ToArray();
            var json = JsonConvert.SerializeObject(list);
            return json;
        }

    }
}