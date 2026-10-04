using gAPI.Core.Server.Authentication;
using gAPI.Core.Server.Entities;
using TinderWithStats.Backend.Entities;

namespace TinderWithStats.Backend.Mappings;

public class StateMapping : AuthenticationStateMapping<User, Shared.Dtos.State>
{
    public override async Task<Shared.Dtos.State> ToDtoAsync(
        User? dbUser,
        UserToken<User>? dbToken,
        Ip<User>? dbIp,
        Shared.Dtos.State? receivedClientState,
        CancellationToken ct)
    {
        var state = await base.ToDtoAsync(dbUser, dbToken, dbIp, receivedClientState, ct);
        state.IsAdmin = dbUser?.IsAdmin ?? false;
        return state;
    }
}