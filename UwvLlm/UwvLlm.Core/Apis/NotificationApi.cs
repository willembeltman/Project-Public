using gAPI.Core.Server;
using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.Core.Apis;

public class NotificationApi(
    IAuthenticationService<Infrastructure.Data.User, State> authentication)
    : INotificationApi
{
    public async Task<Notification[]> GetNotificationList(CancellationToken ct)
    {
        return [];
    }
}
