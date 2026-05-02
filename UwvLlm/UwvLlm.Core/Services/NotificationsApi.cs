using gAPI.Core.Server;
using UwvLlm.Core.Infrastructure.Data;
using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.Core.Services;

public class NotificationsApi(
    IAuthenticationService<User, State> authentication)
    : INotificationsApi
{
    public async Task<int> GetNotificationCount()
    {
        if (authentication.AuthenticationState.User == null)
            return 0;
        throw new NotImplementedException();
    }
}
