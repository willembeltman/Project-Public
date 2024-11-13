using LanCloud.Enums;

namespace LanCloud.Models.Application.Requests
{
    public interface IApplicationRequest
    {
        ApplicationMessageEnum MessageType { get; }
    }
}