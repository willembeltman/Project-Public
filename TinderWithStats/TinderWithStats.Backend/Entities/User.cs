using gAPI.Core.Attributes;
using gAPI.Core.Server.Entities;

namespace TinderWithStats.Backend.Entities;


[IsAuthorized]
public class User : AuthUser
{
    public virtual ICollection<Profile>? Profiles { get; set; }
    public virtual ICollection<UserRole>? UserRoles { get; set; }
}