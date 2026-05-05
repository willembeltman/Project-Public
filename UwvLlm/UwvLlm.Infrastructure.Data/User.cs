using gAPI.Core.Server.Entities;

namespace UwvLlm.Infrastructure.Data;

public class User : AuthUser
{
    public virtual ICollection<UserNotification>? Notifications { get; set; }
}
