using gAPI.Attributes;

namespace UwvLlm.Shared.Interfaces;

[GenerateApi]
public interface INotificationsApi
{
    Task<int> GetNotificationCount();
}
