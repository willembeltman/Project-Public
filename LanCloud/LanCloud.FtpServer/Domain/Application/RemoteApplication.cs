using LanCloud.Models.Application.Requests;
using LanCloud.Models.Application.Responses;
using LanCloud.Servers.Wjp;
using Newtonsoft.Json;
using LanCloud.Shared.Log;
using LanCloud.Models.Configs;

namespace LanCloud.Domain.Application
{
    public class RemoteApplication : WjpProxy
    {
        public RemoteApplication(RemoteApplicationConfig config, ILogger logger) : base(config)
        {
            Config = config;
            Logger = logger;

            //Logger.Info($"Loaded");
        }

        public RemoteApplicationConfig Config { get; }
        public ILogger Logger { get; }

        public PingResponse Ping()
        {
            var request = new PingRequest();
            var json = JsonConvert.SerializeObject(request);
            var wjpRequest = new WjpRequest((int)request.MessageType, json, null);
            var wjpResponse = SendRequest(wjpRequest);
            var response = JsonConvert.DeserializeObject<PingResponse>(wjpResponse.Json);
            return response;
        }
        public GetExternalSharesResponse GetShares()
        {
            GetExternalSharesRequest request = new GetExternalSharesRequest();
            var json = JsonConvert.SerializeObject(request);
            var wjpRequest = new WjpRequest((int)request.MessageType, json, null);
            var wjpResponse = SendRequest(wjpRequest);
            var response = JsonConvert.DeserializeObject<GetExternalSharesResponse>(wjpResponse.Json);
            return response;
        }
    }
}
