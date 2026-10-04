using gAPI.Core.Server.Extensions;

namespace TinderWithStats.Backend.Mappings;

public class UsersMapping(
    gAPI.Core.Interfaces.IUseCase<TinderWithStats.Backend.Entities.User, TinderWithStats.Shared.Dtos.User, Guid> useCase) 
    : gAPI.Core.Interfaces.Mapping<TinderWithStats.Backend.Entities.User, TinderWithStats.Shared.Dtos.User>
{
    public override TinderWithStats.Backend.Entities.User ToEntity(
        TinderWithStats.Shared.Dtos.User dto, 
        TinderWithStats.Backend.Entities.User entity)
    {
        entity.IsAdmin = dto.IsAdmin;
        entity.Id = dto.Id;
        entity.UserName = dto.UserName;
        entity.Email = dto.Email;
        entity.PhoneNumber = dto.PhoneNumber;

        return entity;
    }

    public override async Task<TinderWithStats.Shared.Dtos.User> ToDtoAsync(
        TinderWithStats.Backend.Entities.User entity, 
        TinderWithStats.Shared.Dtos.User dto,
        CancellationToken ct)
    {
        dto.IsAdmin = entity.IsAdmin;
        dto.Id = entity.Id;
        dto.UserName = entity.UserName;
        dto.Email = entity.Email;
        dto.PhoneNumber = entity.PhoneNumber;
        
        await ExtendDto(dto, ct);

        return dto;
    }

    public override IAsyncEnumerable<TinderWithStats.Shared.Dtos.User> ProjectToDtosAsync(
        IQueryable<TinderWithStats.Backend.Entities.User> entities,
        string[]? orderby, 
        int? skip, 
        int? take,
        CancellationToken ct)
    {  
        var dtos = entities
            .Select(entity => new TinderWithStats.Shared.Dtos.User()
            {
                IsAdmin = entity.IsAdmin,
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
        TinderWithStats.Shared.Dtos.User dto,
        CancellationToken ct)
    {
        dto.CanUpdate = await useCase.CanUpdateAsync(dto, ct);
        dto.CanDelete = await useCase.CanDeleteAsync(dto, ct);
    }
}