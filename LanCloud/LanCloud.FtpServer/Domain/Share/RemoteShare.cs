using LanCloud.Domain.Application;
using LanCloud.Models.Dtos;
using LanCloud.Models.Share.Requests;
using LanCloud.Models.Share.Responses;
using LanCloud.Servers.Wjp;
using LanCloud.Shared.Log;
using Newtonsoft.Json;

namespace LanCloud.Domain.Share
{
    public class RemoteShare : WjpProxy, IShare
    {
        public RemoteShare(RemoteApplication remoteApplication, ShareDto config, ILogger logger) : base(config, remoteApplication.Application, logger)
        {
            Logger = logger;
        }

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
    }
}
