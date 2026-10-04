using gAPI.Core.Server.Extensions;

namespace TinderWithStats.Backend.Mappings;

public class LocationsMapping(
    gAPI.Core.Interfaces.IUseCase<TinderWithStats.Backend.Entities.Location, TinderWithStats.Shared.Dtos.Location, int> useCase) 
    : gAPI.Core.Interfaces.Mapping<TinderWithStats.Backend.Entities.Location, TinderWithStats.Shared.Dtos.Location>
{
    public override TinderWithStats.Backend.Entities.Location ToEntity(
        TinderWithStats.Shared.Dtos.Location dto, 
        TinderWithStats.Backend.Entities.Location entity)
    {
        entity.Id = dto.Id;
        entity.LocationName = dto.LocationName;

        return entity;
    }

    public override async Task<TinderWithStats.Shared.Dtos.Location> ToDtoAsync(
        TinderWithStats.Backend.Entities.Location entity, 
        TinderWithStats.Shared.Dtos.Location dto,
        CancellationToken ct)
    {
        dto.Id = entity.Id;
        dto.LocationName = entity.LocationName;
        
        await ExtendDto(dto, ct);

        return dto;
    }

    public override IAsyncEnumerable<TinderWithStats.Shared.Dtos.Location> ProjectToDtosAsync(
        IQueryable<TinderWithStats.Backend.Entities.Location> entities,
        string[]? orderby, 
        int? skip, 
        int? take,
        CancellationToken ct)
    {  
        var dtos = entities
            .Select(entity => new TinderWithStats.Shared.Dtos.Location()
            {
                Id = entity.Id,
                LocationName = entity.LocationName,
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
        TinderWithStats.Shared.Dtos.Location dto,
        CancellationToken ct)
    {
        dto.CanUpdate = await useCase.CanUpdateAsync(dto, ct);
        dto.CanDelete = await useCase.CanDeleteAsync(dto, ct);
    }
}