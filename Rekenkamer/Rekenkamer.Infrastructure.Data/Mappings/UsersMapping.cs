using gAPI.Core.Server.Extensions;

namespace Rekenkamer.Infrastructure.Data.Mappings;

public class UsersMapping(
    gAPI.Core.Interfaces.IUseCase<Rekenkamer.Infrastructure.Data.Entities.User, Rekenkamer.Shared.Dtos.User, Guid> useCase) 
    : gAPI.Core.Interfaces.Mapping<Rekenkamer.Infrastructure.Data.Entities.User, Rekenkamer.Shared.Dtos.User>
{
    public override Rekenkamer.Infrastructure.Data.Entities.User ToEntity(
        Rekenkamer.Shared.Dtos.User dto, 
        Rekenkamer.Infrastructure.Data.Entities.User entity)
    {
        entity.Id = dto.Id;
        entity.UserName = dto.UserName;
        entity.Email = dto.Email;
        entity.PhoneNumber = dto.PhoneNumber;

        return entity;
    }

    public override async Task<Rekenkamer.Shared.Dtos.User> ToDtoAsync(
        Rekenkamer.Infrastructure.Data.Entities.User entity, 
        Rekenkamer.Shared.Dtos.User dto,
        CancellationToken ct)
    {
        dto.Id = entity.Id;
        dto.UserName = entity.UserName;
        dto.Email = entity.Email;
        dto.PhoneNumber = entity.PhoneNumber;
        
        await ExtendDto(dto, ct);

        return dto;
    }

    public override IAsyncEnumerable<Rekenkamer.Shared.Dtos.User> ProjectToDtosAsync(
        IQueryable<Rekenkamer.Infrastructure.Data.Entities.User> entities,
        string[]? orderby, 
        int? skip, 
        int? take,
        CancellationToken ct)
    {  
        var dtos = entities
            .Select(entity => new Rekenkamer.Shared.Dtos.User()
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
        Rekenkamer.Shared.Dtos.User dto,
        CancellationToken ct)
    {
        dto.CanUpdate = await useCase.CanUpdateAsync(dto, ct);
        dto.CanDelete = await useCase.CanDeleteAsync(dto, ct);
    }
}