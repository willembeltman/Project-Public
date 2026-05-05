using gAPI.Core.Attributes;
using UwvLlm.Shared.Dtos;

namespace UwvLlm.Shared.Interfaces;

[GenerateHub]
[IsAuthorized]
public interface INotificationHub
{
    Task OnNotificationReceived(UserNotification notification);
}
