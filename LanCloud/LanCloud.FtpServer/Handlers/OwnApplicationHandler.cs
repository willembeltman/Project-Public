using LanCloud.ApplicationServer;
using LanCloud.ApplicationServer.Interfaces;
using LanCloud.Dtos;
using LanCloud.Models;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace LanCloud.Handlers
{
    internal class OwnApplicationHandler : IApplicationHandler
    {
        private FolderCollection OwnFtps;
        private ExternalApplicationCollection ExternalApplications;
        private ExternalFtpCollection ExternalFtps;

        public OwnApplicationHandler(
            FolderCollection ownFtpShares, 
            ExternalApplicationCollection externalApplications, 
            ExternalFtpCollection externalFtpShares)
        {
            this.OwnFtps = ownFtpShares;
            this.ExternalApplications = externalApplications;
            this.ExternalFtps = externalFtpShares;
        }

        public string Receive(string request)
        {
            switch (request) {
                case "Ping":
                    return Handle_Ping();
                case "GetOwnFtpsAsExternalFtps":
                    return Handle_GetOwnFtpsAsExternalFtps();
                default:
                    throw new NotImplementedException();
            }
        }

        private string Handle_Ping()
        {
            return "Pong";
        }
        private string Handle_GetOwnFtpsAsExternalFtps()
        {
            var list = OwnFtps
                .Select(a => new ExternalFtpDto()
                {
                    
                    //Url = a.LocalShare
                })
                .ToArray();
            var json = JsonConvert.SerializeObject(list);
            return json;
        }

    }
}