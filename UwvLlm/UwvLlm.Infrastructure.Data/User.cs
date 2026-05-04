using gAPI.Core.Server.Entities;

namespace UwvLlm.Api.Core.Infrastructure.Data;

public class User : AuthUser
{
    public virtual ICollection<UserNotification>? Notifications { get; set; }
}
