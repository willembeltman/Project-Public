using LanCloud.Enums;

namespace LanCloud.Models.Share.Requests
{
    public interface IShareRequest
    {
        ShareMessageEnum MessageType { get; }
    }
}