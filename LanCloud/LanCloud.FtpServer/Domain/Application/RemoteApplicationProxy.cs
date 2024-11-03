using LanCloud.Models.Requests;
using LanCloud.Models.Responses;
using LanCloud.Servers.Wjp;
using Newtonsoft.Json;
using LanCloud.Shared.Log;
using LanCloud.Models.Configs;

namespace LanCloud.Domain.Application
{
    public class RemoteApplicationProxy : WjpProxy
    {
        public RemoteApplicationConfig Config { get; }
        public ILogger Logger { get; }

        public RemoteApplicationProxy(RemoteApplicationConfig config, ILogger logger) : base(config)
        {
            Config = config;
            Logger = logger;

            Logger.Info($"Loaded");
        }

        public PingResponse Ping()
        {
            var request = new PingRequest();
            var json = JsonConvert.SerializeObject(request);
            var wjpRequest = new WjpRequest(request.MessageType, json, null);
            var wjpResponse = SendRequest(wjpRequest);
            var response = JsonConvert.DeserializeObject<PingResponse>(wjpResponse.Json);
            return response;
        }
        public GetExternalSharesResponse GetShares()
        {
            GetExternalSharesRequest request = new GetExternalSharesRequest();
            var json = JsonConvert.SerializeObject(request);
            var wjpRequest = new WjpRequest(request.MessageType, json, null);
            var wjpResponse = SendRequest(wjpRequest);
            var response = JsonConvert.DeserializeObject<GetExternalSharesResponse>(wjpResponse.Json);
            return response;
        }
        //public PingResponse Ping(IApplicationRequest request, byte[] data = null)
        //{
        //    var json = JsonConvert.SerializeObject(request);
        //    var wjpRequest = new WjpRequest(request.MessageType, json, data);
        //    var wjpResponse = SendRequest(wjpRequest);
        //    var response = JsonConvert.DeserializeObject<PingResponse>(wjpResponse.Json);
        //    return response;
        //}
    }
}
