using gAPI.Attributes;
using UwvLlm.Shared.Dtos;

namespace UwvLlm.Shared.Interfaces;

[GenerateApi]
public interface INotificationApi
{
    Task<Notification[]> GetNotificationList(CancellationToken ct);
}
