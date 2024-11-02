using LanCloud.Models.Requests;
using LanCloud.Models.Responses;
using LanCloud.Configs;
using LanCloud.Servers.Wjp;
using Newtonsoft.Json;

namespace LanCloud.Domain.Application
{
    public class RemoteApplicationProxy : WjpProxy
    {
        public RemoteApplicationProxy(RemoteApplicationConfig config) : base(config)
        {
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
