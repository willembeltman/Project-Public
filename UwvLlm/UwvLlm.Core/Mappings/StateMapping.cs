using gAPI.Core.Server.Entities;
using gAPI.Core.Server.Mappings;

namespace UwvLlm.Core.Mappings;

public class StateMapping(
    IStateUserMapping<Infrastructure.Data.User, UwvLlm.Shared.Dtos.StateUser> stateUserMapping)
    : IStateMapping<Infrastructure.Data.User, UwvLlm.Shared.Dtos.State>
{
    public async Task<UwvLlm.Shared.Dtos.State> ToDtoAsync(
        Infrastructure.Data.User? dbUser, 
        UserToken<Infrastructure.Data.User>? dbToken, 
        Ip<Infrastructure.Data.User> dbIp,
        UwvLlm.Shared.Dtos.State? receivedClientState, 
        CancellationToken ct)
    {
        return new UwvLlm.Shared.Dtos.State
        {
            User = dbUser != null ? await stateUserMapping.ToDtoAsync(dbUser, new UwvLlm.Shared.Dtos.StateUser(), ct) : null
        };
    }
}
