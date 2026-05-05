using gAPI.Core.Server.Mappings;
using gAPI.Core.Server.Storage;
using UwvLlm.Shared.Dtos;

namespace UwvLlm.Infrastructure.Data.Mappings;

public class StateUserMapping(
    IStorageService storageService) :
    IStateUserMapping<UwvLlm.Infrastructure.Data.Entities.User, StateUser>
{
    public async Task<StateUser> ToDtoAsync(UwvLlm.Infrastructure.Data.Entities.User entity, StateUser dto, CancellationToken ct)
    {
        dto.Id = entity.Id;
        dto.UserName = entity.UserName;
        dto.Email = entity.Email;
        dto.StorageFileUrl = await storageService.GetStorageFileUrlAsync(dto.Id.ToString(), "User", ct);
        return dto;
    }
}