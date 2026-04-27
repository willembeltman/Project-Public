using gAPI.Core.Server.Entities;
using gAPI.Core.Server.Mappings;
using UwvLlm.Core.Infrastructure.Data;
using UwvLlm.Shared.Dtos;

namespace UwvLlm.Core.Mappings;

public class StateMapping(
    IStateUserMapping<User, StateUser> stateUserMapping)
    : IStateMapping<User, State>
{
    public async Task<State> ToDtoAsync(User? dbUser, UserToken<User>? dbToken, Ip<User> dbIp, State? receivedClientState, CancellationToken ct)
    {
        return new State
        {
            User = dbUser != null ? await stateUserMapping.ToDtoAsync(dbUser, new StateUser(), ct) : null
        };
    }
}
