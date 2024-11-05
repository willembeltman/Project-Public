using LanCloud.Enums;

namespace LanCloud.Models.Application.Requests
{
    public class GetExternalSharesRequest : IApplicationRequest
    {
        public ApplicationMessageEnum MessageType => ApplicationMessageEnum.GetExternalShares;
    }
}