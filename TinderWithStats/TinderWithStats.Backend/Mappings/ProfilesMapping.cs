using gAPI.Core.Server.Extensions;

namespace TinderWithStats.Backend.Mappings;

public class ProfilesMapping(
    gAPI.Core.Interfaces.IUseCase<TinderWithStats.Backend.Entities.Profile, TinderWithStats.Shared.Dtos.Profile, Guid> useCase) 
    : gAPI.Core.Interfaces.Mapping<TinderWithStats.Backend.Entities.Profile, TinderWithStats.Shared.Dtos.Profile>
{
    public override TinderWithStats.Backend.Entities.Profile ToEntity(
        TinderWithStats.Shared.Dtos.Profile dto, 
        TinderWithStats.Backend.Entities.Profile entity)
    {
        entity.Id = dto.Id;
        entity.UserId = dto.UserId;
        entity.LocationId = dto.LocationId;
        entity.DateOfBirth = dto.DateOfBirth;
        entity.Length = dto.Length;
        entity.Beroep = dto.Beroep;
        entity.Bio = dto.Bio;

        return entity;
    }

    public override async Task<TinderWithStats.Shared.Dtos.Profile> ToDtoAsync(
        TinderWithStats.Backend.Entities.Profile entity, 
        TinderWithStats.Shared.Dtos.Profile dto,
        CancellationToken ct)
    {
        dto.Id = entity.Id;
        dto.UserId = entity.UserId;
        dto.LocationId = entity.LocationId;
        dto.DateOfBirth = entity.DateOfBirth;
        dto.Length = entity.Length;
        dto.Beroep = entity.Beroep;
        dto.Bio = entity.Bio;
        
        dto.UserName = 
            ("" + (entity?.User?.UserName ?? default) + "") + " " + 
                (" (" + (entity?.User?.Email ?? default) + ")");

        await ExtendDto(dto, ct);

        return dto;
    }

    public override IAsyncEnumerable<TinderWithStats.Shared.Dtos.Profile> ProjectToDtosAsync(
        IQueryable<TinderWithStats.Backend.Entities.Profile> entities,
        string[]? orderby, 
        int? skip, 
        int? take,
        CancellationToken ct)
    {  
        var dtos = entities
            .Select(entity => new TinderWithStats.Shared.Dtos.Profile()
            {
                Id = entity.Id,
                UserId = entity.UserId,
                LocationId = entity.LocationId,
                DateOfBirth = entity.DateOfBirth,
                Length = entity.Length,
                Beroep = entity.Beroep,
                Bio = entity.Bio,
#nullable disable
                UserName = 
                    ("" + entity.User.UserName + "") + " " + 
                        (" (" + entity.User.Email + ")"),
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
        TinderWithStats.Shared.Dtos.Profile dto,
        CancellationToken ct)
    {
        dto.CanUpdate = await useCase.CanUpdateAsync(dto, ct);
        dto.CanDelete = await useCase.CanDeleteAsync(dto, ct);
    }
}