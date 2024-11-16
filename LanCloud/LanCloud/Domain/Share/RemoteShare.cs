using LanCloud.Domain.Application;
using LanCloud.Enums;
using LanCloud.Models.Dtos;
using LanCloud.Models.Share.Requests;
using LanCloud.Models.Share.Responses;
using LanCloud.Servers.Wjp;
using LanCloud.Shared.Log;
using Newtonsoft.Json;
using System.Linq;

namespace LanCloud.Domain.Share
{
    public class RemoteShare : WjpProxy, IShare
    {
        public RemoteShare(RemoteApplication remoteApplication, ShareDto config, ILogger logger) : base(config, remoteApplication.LocalApplication, logger)
        {
            Logger = logger;
            ShareStripes = config.ShareStripes
                .Select(a => new RemoteShareStripe(this, a, logger))
                .ToArray();
        }

        public ILogger Logger { get; }
        public RemoteShareStripe[] ShareStripes { get; }

        IShareStripe[] IShare.ShareStripes => ShareStripes;

        public FileStripeDto[] ListFileBits()
        {
            string responseJson = "";
            int responseDataLength = 0;
            SendRequest((int)ShareMessageEnum.ListFileBits, null, null, 0, out responseJson, null, out responseDataLength);
            var response = JsonConvert.DeserializeObject<FileStripeDto[]>(responseJson);
            return response;
        }
        public CreateFileBitSessionResponse CreateFileBitSession(string path)
        {
            var request = new CreateFileBitSessionRequest(path);
            var requestJson = JsonConvert.SerializeObject(request);

            string responseJson = "";
            int responseDataLength = 0;
            SendRequest((int)ShareMessageEnum.CreateFileBitSession, null, null, 0, out responseJson, null, out responseDataLength);
            var response = JsonConvert.DeserializeObject<CreateFileBitSessionResponse>(responseJson);
            return response;
        }
        public StoreFileBitPartResponse StoreFileBitPart(string path, long index, byte[] data, int datalength)
        {
            var request = new StoreFileBitPartRequest(path, index);
            var requestJson = JsonConvert.SerializeObject(request);

            string responseJson = "";
            int responseDataLength = 0;
            SendRequest((int)ShareMessageEnum.StoreFileBitPart, null, null, 0, out responseJson, null, out responseDataLength);
            var response = JsonConvert.DeserializeObject<StoreFileBitPartResponse>(responseJson);
            return response;
        }
        public CloseFileBitSessionResponse CloseFileBitSession(string path, long index, byte[] data, int datalength)
        {
            var request = new CloseFileBitSessionRequest(path, index);
            var requestJson = JsonConvert.SerializeObject(request);

            string responseJson = "";
            int responseDataLength = 0;
            SendRequest((int)ShareMessageEnum.CloseFileBitSession, null, null, 0, out responseJson, null, out responseDataLength);
            var response = JsonConvert.DeserializeObject<CloseFileBitSessionResponse>(responseJson);
            return response;
        }
    }
}
