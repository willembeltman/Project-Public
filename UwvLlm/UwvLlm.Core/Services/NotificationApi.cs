using gAPI.Core.Server;
using UwvLlm.Core.Infrastructure.Data;
using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.Core.Services;

public class NotificationApi(
    IAuthenticationService<User, StateDto> authentication)
    : INotificationApi
{
    public async Task<NotificationDto[]> GetNotificationList(CancellationToken ct)
    {
        return [];
    }
}
