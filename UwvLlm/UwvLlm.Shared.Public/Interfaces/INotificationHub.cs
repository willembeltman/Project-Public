using gAPI.Core.Attributes;
using UwvLlm.Shared.Public.Dtos;

namespace UwvLlm.Shared.Public.Interfaces;

[GenerateHub]
[IsAuthorized]
public interface INotificationHub
{
    Task OnNotificationReceived(UserNotification notification);
}
