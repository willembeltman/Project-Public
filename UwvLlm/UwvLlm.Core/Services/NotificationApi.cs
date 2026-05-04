using gAPI.Core.Server;
using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.Core.Services;

public class NotificationApi(
    IAuthenticationService<Infrastructure.Data.User, StateDto> authentication)
    : INotificationApi
{
    public async Task<NotificationDto[]> GetNotificationList(CancellationToken ct)
    {
        return [];
    }
}
