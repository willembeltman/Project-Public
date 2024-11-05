using LanCloud.Enums;

namespace LanCloud.Models.Application.Requests
{
    public class PingRequest : IApplicationRequest
    {
        public ApplicationMessageEnum MessageType => ApplicationMessageEnum.Ping;
    }
}
