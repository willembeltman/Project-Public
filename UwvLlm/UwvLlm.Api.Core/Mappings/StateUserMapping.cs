using gAPI.Core.Server.Mappings;
using gAPI.Core.Server.Storage;
using UwvLlm.Shared.Public.Dtos;

namespace UwvLlm.Api.Core.Mappings;

public class StateUserMapping(
    IStorageService storageService) :
    IStateUserMapping<Infrastructure.Data.User, StateUser>
{
    public async Task<StateUser> ToDtoAsync(Infrastructure.Data.User entity, StateUser dto, CancellationToken ct)
    {
        dto.Id = entity.Id;
        dto.UserName = entity.UserName;
        dto.Email = entity.Email;
        dto.StorageFileUrl = await storageService.GetStorageFileUrlAsync(dto.Id.ToString(), "User", ct);
        return dto;
    }
}