using LanCloud.Enums;

namespace LanCloud.Communication.Requests
{
    public class PingRequest : IApplicationRequest
    {
        public int MessageType => (int)ApplicationMessageEnum.Ping;
    }
}
