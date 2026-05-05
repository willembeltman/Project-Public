using gAPI.Core.Server.Entities;
using gAPI.Core.Server.Mappings;
using UwvLlm.Infrastructure.Data.Entities;

namespace UwvLlm.Api.Core.Mappings;

public class StateMapping(
    IStateUserMapping<User, Shared.Dtos.StateUser> stateUserMapping)
    : IStateMapping<User, Shared.Dtos.State>
{
    public async Task<Shared.Dtos.State> ToDtoAsync(
        User? dbUser, 
        UserToken<User>? dbToken, 
        Ip<User> dbIp,
        Shared.Dtos.State? receivedClientState, 
        CancellationToken ct)
    {
        return new Shared.Dtos.State
        {
            User = dbUser != null ? await stateUserMapping.ToDtoAsync(dbUser, new Shared.Dtos.StateUser(), ct) : null
        };
    }
}
