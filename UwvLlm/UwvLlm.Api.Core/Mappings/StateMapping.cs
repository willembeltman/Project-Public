using gAPI.Core.Server.Entities;
using gAPI.Core.Server.Mappings;

namespace UwvLlm.Api.Core.Mappings;

public class StateMapping(
    IStateUserMapping<Infrastructure.Data.User, Shared.Dtos.StateUser> stateUserMapping)
    : IStateMapping<Infrastructure.Data.User, Shared.Dtos.State>
{
    public async Task<Shared.Dtos.State> ToDtoAsync(
        Infrastructure.Data.User? dbUser, 
        UserToken<Infrastructure.Data.User>? dbToken, 
        Ip<Infrastructure.Data.User> dbIp,
        Shared.Dtos.State? receivedClientState, 
        CancellationToken ct)
    {
        return new Shared.Dtos.State
        {
            User = dbUser != null ? await stateUserMapping.ToDtoAsync(dbUser, new Shared.Dtos.StateUser(), ct) : null
        };
    }
}
