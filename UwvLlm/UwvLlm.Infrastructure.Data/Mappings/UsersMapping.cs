using gAPI.Core.Server.Extensions;
using gAPI.Core.Server.Storage;
using UwvLlm.Infrastructure.Data.Entities;

namespace UwvLlm.Infrastructure.Data.Mappings;

public class UsersMapping(
    gAPI.Core.Interfaces.IUseCase<User, Shared.Dtos.User, Guid> useCase, 
    IStorageService storageService) 
    : gAPI.Core.Interfaces.Mapping<User, Shared.Dtos.User>
{
    public override User ToEntity(
        Shared.Dtos.User dto, 
        User entity)
    {
        entity.Id = dto.Id;
        entity.UserName = dto.UserName;
        entity.Email = dto.Email;
        entity.PhoneNumber = dto.PhoneNumber;

        return entity;
    }

    public override async Task<Shared.Dtos.User> ToDtoAsync(
        User entity, 
        Shared.Dtos.User dto,
        CancellationToken ct)
    {
        dto.Id = entity.Id;
        dto.UserName = entity.UserName;
        dto.Email = entity.Email;
        dto.PhoneNumber = entity.PhoneNumber;
        
        await ExtendDto(dto, ct);

        return dto;
    }

    public override IAsyncEnumerable<Shared.Dtos.User> ProjectToDtosAsync(
        IQueryable<User> entities,
        string[]? orderby, 
        int? skip, 
        int? take,
        CancellationToken ct)
    {  
        var dtos = entities
            .Select(entity => new Shared.Dtos.User()
            {
                Id = entity.Id,
                UserName = entity.UserName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
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
        Shared.Dtos.User dto,
        CancellationToken ct)
    {
        dto.StorageFileUrl = await storageService.GetStorageFileUrlAsync(dto.Id.ToString(), "User", ct);
        dto.CanUpdate = await useCase.CanUpdateAsync(dto, ct);
        dto.CanDelete = await useCase.CanDeleteAsync(dto, ct);
    }
}