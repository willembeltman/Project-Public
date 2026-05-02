using gAPI.Core.Server;
using UwvLlm.Core.Infrastructure.Data;
using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.Core.Services;

public class NotificationApi(
    IAuthenticationService<User, State> authentication)
    : INotificationApi
{
    public async Task<NotificationDto[]> GetNotificationList()
    {
        return [];
    }
}
