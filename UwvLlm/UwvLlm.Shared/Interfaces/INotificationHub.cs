using gAPI.Attributes;
using UwvLlm.Shared.Dtos;

namespace UwvLlm.Shared.Interfaces;

[GenerateHub]
public interface INotificationHub
{
    Task OnNotificationReceived(UserNotification notification);
}
