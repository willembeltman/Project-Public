using LanCloud.Enums;

namespace LanCloud.Models.Share.Requests
{
    public class PingRequest : IShareRequest
    {
        public ShareMessageEnum MessageType => ShareMessageEnum.Ping;
    }
}
