using gAPI.Core.Server.Storage;
namespace UwvLlm.Infrastructure.Data.Mappings;

public class StateMapping (
    IStorageService storageService)
{

    public async Task<UwvLlm.Shared.Dtos.StateUser> ToDtoAsync(
        UwvLlm.Infrastructure.Data.Entities.User entity,
        UwvLlm.Shared.Dtos.StateUser dto,
        CancellationToken ct)
    {
        dto.Id = entity.Id;
        dto.UserName = entity.UserName;
        dto.Email = entity.Email;
        dto.StorageFileUrl = await storageService.GetStorageFileUrlAsync(dto.Id.ToString(), "StateUser", ct);
        return dto;
    }


}