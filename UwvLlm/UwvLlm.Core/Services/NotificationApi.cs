using gAPI.Core.Server;
using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.Core.Services;

public class NotificationApi(
    IAuthenticationService<Infrastructure.Data.User, State> authentication)
    : INotificationApi
{
    public async Task<UserNotification[]> GetNotificationList(CancellationToken ct)
    {
        var bla = authentication.Result;
        if (bla == null)
        {

        }
        return [];
    }
}
