using LanCloud.Shared.Dtos;
using LanCloud.Shared.Interfaces;
using LanCloud.Shared.Models;
using Newtonsoft.Json;

namespace LanCloud
{
    public class LocalShareHandler : ILocalShareHandler
    {
        public LocalShareHandler(LocalShareConfig shareConfig)
        {
            ShareConfig = shareConfig;
        }

        public LocalShareConfig ShareConfig { get; }

        public RemoteShareResponse Receive(LocalShareRequest request)
        {

            var requestDto = JsonConvert.DeserializeObject<ShareRequestDto>(request.Json);
            throw new System.NotImplementedException();
        }
    }
}