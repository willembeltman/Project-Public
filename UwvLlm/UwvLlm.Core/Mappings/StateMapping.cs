using gAPI.Core.Server.Entities;
using gAPI.Core.Server.Mappings;
using UwvLlm.Core.Infrastructure.Data;
using UwvLlm.Shared.Dtos;

namespace UwvLlm.Core.Mappings;

public class StateMapping(
    IStateUserMapping<User, StateUserDto> stateUserMapping)
    : IStateMapping<User, StateDto>
{
    public async Task<StateDto> ToDtoAsync(User? dbUser, UserToken<User>? dbToken, Ip<User> dbIp, StateDto? receivedClientState, CancellationToken ct)
    {
        return new StateDto
        {
            User = dbUser != null ? await stateUserMapping.ToDtoAsync(dbUser, new StateUserDto(), ct) : null
        };
    }
}
