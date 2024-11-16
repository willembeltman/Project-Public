using LanCloud.Enums;
using LanCloud.Models.Dtos;
using LanCloud.Models.Share.Requests;
using LanCloud.Models.Share.Responses;
using LanCloud.Shared.Log;
using Newtonsoft.Json;

namespace LanCloud.Domain.Share
{
    public class RemoteShareStripe : IShareStripe
    {
        public RemoteShareStripe(RemoteShare remoteShare, ShareStripeDto config, ILogger logger) 
        {
            RemoteShare = remoteShare;
            Config = config;
            Logger = logger;
        }

        public RemoteShare RemoteShare { get; }
        public ShareStripeDto Config { get; }
        public ILogger Logger { get; }

        public IShare Share => RemoteShare;
        public byte[] Indexes => Config.Indexes;

        public CreateFileBitSessionResponse CreateFileBitSession(string path)
        {
            var request = new CreateFileBitSessionRequest(path);
            var requestJson = JsonConvert.SerializeObject(request);

            string responseJson = "";
            int responseDataLength = 0;
            RemoteShare.SendRequest((int)ShareMessageEnum.CreateFileBitSession, null, null, 0, out responseJson, null, out responseDataLength);
            var response = JsonConvert.DeserializeObject<CreateFileBitSessionResponse>(responseJson);
            return response;
        }
        public StoreFileBitPartResponse StoreFileBitPart(string path, long index, byte[] data, int datalength)
        {
            var request = new StoreFileBitPartRequest(path, index);
            var requestJson = JsonConvert.SerializeObject(request);

            string responseJson = "";
            int responseDataLength = 0;
            RemoteShare.SendRequest((int)ShareMessageEnum.StoreFileBitPart, null, null, 0, out responseJson, null, out responseDataLength);
            var response = JsonConvert.DeserializeObject<StoreFileBitPartResponse>(responseJson);
            return response;
        }
        public CloseFileBitSessionResponse CloseFileBitSession(string path, long index, byte[] data, int datalength)
        {
            var request = new CloseFileBitSessionRequest(path, index);
            var requestJson = JsonConvert.SerializeObject(request);

            string responseJson = "";
            int responseDataLength = 0;
            RemoteShare.SendRequest((int)ShareMessageEnum.CloseFileBitSession, null, null, 0, out responseJson, null, out responseDataLength);
            var response = JsonConvert.DeserializeObject<CloseFileBitSessionResponse>(responseJson);
            return response;
        }
    }
}
