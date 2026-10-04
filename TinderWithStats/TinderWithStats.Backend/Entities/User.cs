using gAPI.Core.Attributes;
using gAPI.Core.Server.Entities;
using gAPI.Core.Server.Storage;

namespace Bsd.Infrastructure.Data.Entities;

[IsAuthorized]
public class User : AuthUser, IStorageFile
{
    public bool IsAdmin { get; set; }
    string IStorageFile.Id => Id.ToString();
}
