using LanCloud.Domain.Application;
using LanCloud.Domain.FileStripe;
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

        public IFileStripe FindFileStripe(string extention, string hash, long length, byte[] indexes)
        {
            var request = new FindFileStripesRequest(extention, hash, length, indexes);
            var requestJson = JsonConvert.SerializeObject(request);

            string responseJson = "";
            int responseDataLength = 0;
            SendRequest((int)ShareMessageEnum.FindFileStripes, requestJson, null, 0, out responseJson, null, out responseDataLength);
            var fileStripeDto = JsonConvert.DeserializeObject<FileStripeDto>(responseJson);
            if (fileStripeDto == null) return null;

            var remoteFileStripe = new RemoteFileStripe(this, fileStripeDto);
            return remoteFileStripe;
        }
        public CreateFileStripeSessionResponse CreateFileStripeSession(string path)
        {
            var request = new CreateFileStripeSessionRequest(path);
            var requestJson = JsonConvert.SerializeObject(request);

            string responseJson = "";
            int responseDataLength = 0;
            SendRequest((int)ShareMessageEnum.CreateFileStripeSession, null, null, 0, out responseJson, null, out responseDataLength);
            var response = JsonConvert.DeserializeObject<CreateFileStripeSessionResponse>(responseJson);
            return response;
        }
        public StoreFileStripePartResponse StoreFileStripeChunk(string path, long index, byte[] data, int datalength)
        {
            var request = new StoreFileStripeChunkRequest(path, index);
            var requestJson = JsonConvert.SerializeObject(request);

            string responseJson = "";
            int responseDataLength = 0;
            SendRequest((int)ShareMessageEnum.StoreFileStripePart, requestJson, data, datalength, out responseJson, null, out responseDataLength);
            var response = JsonConvert.DeserializeObject<StoreFileStripePartResponse>(responseJson);
            return response;
        }
        public CloseFileStripeSessionResponse CloseFileStripeSession(string path, long index)
        {
            var request = new CloseFileStripeSessionRequest(path, index);
            var requestJson = JsonConvert.SerializeObject(request);

            string responseJson = "";
            int responseDataLength = 0;
            SendRequest((int)ShareMessageEnum.CloseFileStripeSession, null, null, 0, out responseJson, null, out responseDataLength);
            var response = JsonConvert.DeserializeObject<CloseFileStripeSessionResponse>(responseJson);
            return response;
        }
    }
}
