using LanCloud.Models.Application.Requests;
using LanCloud.Models.Application.Responses;
using LanCloud.Servers.Wjp;
using Newtonsoft.Json;
using LanCloud.Shared.Log;
using LanCloud.Models.Configs;
using LanCloud.Models.Dtos;
using System.Linq;
using LanCloud.Domain.Share;

namespace LanCloud.Domain.Application
{
    public class RemoteApplication : WjpProxy
    {
        public RemoteApplication(
            LocalApplication localApplication,
            RemoteApplicationConfig config, 
            ILogger logger) : base(config, localApplication, logger)
        {
            LocalApplication = localApplication;
            Config = config;
            Logger = logger;

            RemoteShares = new RemoteShare[0];

            StateChanged += RemoteApplication_StateChanged;
        }

        public LocalApplication LocalApplication { get; }
        public RemoteApplicationConfig Config { get; }
        public ILogger Logger { get; }

        public RemoteShare[] RemoteShares { get; private set; }

        private void RemoteApplication_StateChanged(object sender, System.EventArgs e)
        {
            if (Connected)
            {
                RemoteShares = GetShares()
                    .Select(a => new RemoteShare(this, a, Logger))
                    .ToArray();
            }
            else
            {
                foreach (var item in RemoteShares)
                {
                    item.Dispose();
                }
                RemoteShares = new RemoteShare[0];
            }
        }

        public PingResponse Ping()
        {
            var request = new PingRequest();
            var json = JsonConvert.SerializeObject(request);
            var wjpRequest = new WjpRequest((int)request.MessageType, json, null);
            var wjpResponse = SendRequest(wjpRequest);
            var response = JsonConvert.DeserializeObject<PingResponse>(wjpResponse.Json);
            return response;
        }
        public ShareDto[] GetShares()
        {
            GetExternalSharesRequest request = new GetExternalSharesRequest();
            var json = JsonConvert.SerializeObject(request);
            var wjpRequest = new WjpRequest((int)request.MessageType, json, null);
            var wjpResponse = SendRequest(wjpRequest);
            var response = JsonConvert.DeserializeObject<ShareDto[]>(wjpResponse.Json);
            return response;
        }
    }
}
