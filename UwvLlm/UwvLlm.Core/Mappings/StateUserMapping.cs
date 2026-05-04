using gAPI.Core.Server.Mappings;
using gAPI.Storage;
using UwvLlm.Core.Infrastructure.Data;
using UwvLlm.Shared.Dtos;

namespace UwvLlm.Core.Mappings;

public class StateUserMapping(
    IStorageService storageService) :
    IStateUserMapping<User, StateUserDto>
{
    public async Task<StateUserDto> ToDtoAsync(User entity, StateUserDto dto, CancellationToken ct)
    {
        dto.Id = entity.Id;
        dto.UserName = entity.UserName;
        dto.Email = entity.Email;
        dto.StorageFileUrl = await storageService.GetStorageFileUrlAsync(dto.Id.ToString(), "User", ct);
        return dto;
    }
}