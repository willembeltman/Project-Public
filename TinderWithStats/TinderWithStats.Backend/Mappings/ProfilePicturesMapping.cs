using gAPI.Core.Server.Extensions;
using gAPI.Core.Server.Storage;

namespace TinderWithStats.Backend.Mappings;

public class ProfilePicturesMapping(
    gAPI.Core.Interfaces.IUseCase<TinderWithStats.Backend.Entities.ProfilePicture, TinderWithStats.Shared.Dtos.ProfilePicture, Guid> useCase, 
    IStorageService storageService) 
    : gAPI.Core.Interfaces.Mapping<TinderWithStats.Backend.Entities.ProfilePicture, TinderWithStats.Shared.Dtos.ProfilePicture>
{
    public override TinderWithStats.Backend.Entities.ProfilePicture ToEntity(
        TinderWithStats.Shared.Dtos.ProfilePicture dto, 
        TinderWithStats.Backend.Entities.ProfilePicture entity)
    {
        entity.Id = dto.Id;
        entity.ProfileId = dto.ProfileId;
        entity.UploadDate = dto.UploadDate;

        return entity;
    }

    public override async Task<TinderWithStats.Shared.Dtos.ProfilePicture> ToDtoAsync(
        TinderWithStats.Backend.Entities.ProfilePicture entity, 
        TinderWithStats.Shared.Dtos.ProfilePicture dto,
        CancellationToken ct)
    {
        dto.Id = entity.Id;
        dto.ProfileId = entity.ProfileId;
        dto.UploadDate = entity.UploadDate;
        
        await ExtendDto(dto, ct);

        return dto;
    }

    public override IAsyncEnumerable<TinderWithStats.Shared.Dtos.ProfilePicture> ProjectToDtosAsync(
        IQueryable<TinderWithStats.Backend.Entities.ProfilePicture> entities,
        string[]? orderby, 
        int? skip, 
        int? take,
        CancellationToken ct)
    {  
        var dtos = entities
            .Select(entity => new TinderWithStats.Shared.Dtos.ProfilePicture()
            {
                Id = entity.Id,
                ProfileId = entity.ProfileId,
                UploadDate = entity.UploadDate,
#nullable disable
#nullable enable
            })
            .ApplyOrderBy(orderby);

        if (skip != null)
        {
            dtos = dtos.Skip(skip.Value);
        }
        if (take != null)
        {
            dtos = dtos.Take(take.Value);
        }

        return EnumerateDtosAsync(dtos, ct);
    }

    public override async Task ExtendDto(
        TinderWithStats.Shared.Dtos.ProfilePicture dto,
        CancellationToken ct)
    {
        dto.StorageFileUrl = await storageService.GetStorageFileUrlAsync($"ProfilePicture/{dto.Id}", ct);
        dto.CanUpdate = await useCase.CanUpdateAsync(dto, ct);
        dto.CanDelete = await useCase.CanDeleteAsync(dto, ct);
    }
}