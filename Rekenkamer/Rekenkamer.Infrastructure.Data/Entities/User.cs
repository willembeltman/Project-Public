using gAPI.Core.Attributes;
using gAPI.Core.Server.Entities;

namespace Rekenkamer.Infrastructure.Data.Entities;

[IsAuthorized]
public class User : AuthUser
{
}
