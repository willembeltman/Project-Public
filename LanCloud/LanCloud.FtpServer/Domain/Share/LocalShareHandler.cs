using LanCloud.Shared.Log;
using LanCloud.Servers.Wjp;
using Newtonsoft.Json;
using LanCloud.Models.Dtos;

namespace LanCloud.Domain.Share
{
    public class LocalShareHandler : IWjpHandler
    {
        public LocalShareHandler(LocalShareStorage storage, ILogger logger)
        {
            Storage = storage;
            Logger = logger;

            Logger.Info($"Loaded");
        }

        public LocalShareStorage Storage { get; }
        public ILogger Logger { get; }

        public WjpResponse ProcessRequest(WjpRequest request)
        {
            var requestDto = JsonConvert.DeserializeObject<ShareRequestDto>(request.Json);

            throw new System.NotImplementedException();
        }
    }
}