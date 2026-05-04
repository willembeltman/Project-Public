using gAPI.Core.Server.Entities;
using gAPI.Core.Server.Mappings;

namespace UwvLlm.Core.Mappings;

public class StateMapping(
    IStateUserMapping<Infrastructure.Data.User, UwvLlm.Shared.Dtos.StateUserDto> stateUserMapping)
    : IStateMapping<Infrastructure.Data.User, UwvLlm.Shared.Dtos.StateDto>
{
    public async Task<UwvLlm.Shared.Dtos.StateDto> ToDtoAsync(
        Infrastructure.Data.User? dbUser, 
        UserToken<Infrastructure.Data.User>? dbToken, 
        Ip<Infrastructure.Data.User> dbIp,
        UwvLlm.Shared.Dtos.StateDto? receivedClientState, 
        CancellationToken ct)
    {
        return new UwvLlm.Shared.Dtos.StateDto
        {
            User = dbUser != null ? await stateUserMapping.ToDtoAsync(dbUser, new UwvLlm.Shared.Dtos.StateUserDto(), ct) : null
        };
    }
}
