using LanCloud.Enums;

namespace LanCloud.Communication.Requests
{
    public class GetExternalSharesRequest : IApplicationRequest
    {
        public int MessageType => (int)ApplicationMessageEnum.GetExternalShares;
    }
}