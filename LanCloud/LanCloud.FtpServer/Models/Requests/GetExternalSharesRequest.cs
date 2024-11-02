using LanCloud.Enums;

namespace LanCloud.Models.Requests
{
    public class GetExternalSharesRequest : IApplicationRequest
    {
        public int MessageType => (int)ApplicationMessageEnum.GetExternalShares;
    }
}