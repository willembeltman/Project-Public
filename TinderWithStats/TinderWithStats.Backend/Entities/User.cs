using gAPI.Core.Attributes;
using gAPI.Core.Server.Entities;

namespace TinderWithStats.Backend.Entities;


[IsAuthorized]
public class User : AuthUser
{
    public bool IsAdmin { get; set; }

    public virtual ICollection<Profile>? Profiles { get; set; }
}