using gAPI.Core.Server.Entities;
using gAPI.Core.Server.Mappings;

namespace UwvLlm.Api.Core.Mappings;

public class StateMapping(
    IStateUserMapping<Infrastructure.Data.User, Shared.Public.Dtos.StateUser> stateUserMapping)
    : IStateMapping<Infrastructure.Data.User, Shared.Public.Dtos.State>
{
    public async Task<Shared.Public.Dtos.State> ToDtoAsync(
        Infrastructure.Data.User? dbUser, 
        UserToken<Infrastructure.Data.User>? dbToken, 
        Ip<Infrastructure.Data.User> dbIp,
        Shared.Public.Dtos.State? receivedClientState, 
        CancellationToken ct)
    {
        return new Shared.Public.Dtos.State
        {
            User = dbUser != null ? await stateUserMapping.ToDtoAsync(dbUser, new Shared.Public.Dtos.StateUser(), ct) : null
        };
    }
}
