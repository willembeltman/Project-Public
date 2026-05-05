using gAPI.Core.Server.Entities;
using gAPI.Core.Server.Mappings;

namespace UwvLlm.Api.Core.Mappings;

public class StateMapping(
    IStateUserMapping<UwvLlm.Infrastructure.Data.User, UwvLlm.Shared.Dtos.StateUser> stateUserMapping)
    : IStateMapping<UwvLlm.Infrastructure.Data.User, UwvLlm.Shared.Dtos.State>
{
    public async Task<UwvLlm.Shared.Dtos.State> ToDtoAsync(
        UwvLlm.Infrastructure.Data.User? dbUser, 
        UserToken<UwvLlm.Infrastructure.Data.User>? dbToken, 
        Ip<UwvLlm.Infrastructure.Data.User> dbIp,
        UwvLlm.Shared.Dtos.State? receivedClientState, 
        CancellationToken ct)
    {
        return new UwvLlm.Shared.Dtos.State
        {
            User = dbUser != null ? await stateUserMapping.ToDtoAsync(dbUser, new UwvLlm.Shared.Dtos.StateUser(), ct) : null
        };
    }
}
