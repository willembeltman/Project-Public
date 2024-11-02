using LanCloud.Enums;

namespace LanCloud.Models.Requests
{
    public class PingRequest : IApplicationRequest
    {
        public int MessageType => (int)ApplicationMessageEnum.Ping;
    }
}
