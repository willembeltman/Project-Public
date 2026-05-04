using gAPI.Extensions;

namespace UwvLlm.Core.Mappings;

public class UserNotificationQuickOptionsMapping(
    gAPI.Interfaces.IUseCase<Infrastructure.Data.UserNotificationQuickOption, Shared.Dtos.UserNotificationQuickOption, long> useCase) 
    : gAPI.Interfaces.Mapping<Infrastructure.Data.UserNotificationQuickOption, Shared.Dtos.UserNotificationQuickOption>
{
    public override Infrastructure.Data.UserNotificationQuickOption ToEntity(
        Shared.Dtos.UserNotificationQuickOption dto, 
        Infrastructure.Data.UserNotificationQuickOption entity)
    {
        entity.Id = dto.Id;
        entity.UserNotificationId = dto.UserNotificationId;
        entity.Value = dto.Value;

        return entity;
    }

    public override async Task<Shared.Dtos.UserNotificationQuickOption> ToDtoAsync(
        Infrastructure.Data.UserNotificationQuickOption entity, 
        Shared.Dtos.UserNotificationQuickOption dto,
        CancellationToken ct)
    {
        dto.Id = entity.Id;
        dto.UserNotificationId = entity.UserNotificationId;
        dto.Value = entity.Value;
        

        await ExtendDto(dto, ct);

        return dto;
    }

    public override IAsyncEnumerable<Shared.Dtos.UserNotificationQuickOption> ProjectToDtosAsync(
        IQueryable<Infrastructure.Data.UserNotificationQuickOption> entities,
        string[]? orderby, 
        int? skip, 
        int? take,
        CancellationToken ct)
    {  
        var dtos = entities
            .Select(entity => new Shared.Dtos.UserNotificationQuickOption()
            {
                Id = entity.Id,
                UserNotificationId = entity.UserNotificationId,
                Value = entity.Value
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
        Shared.Dtos.UserNotificationQuickOption dto,
        CancellationToken ct)
    {
        dto.CanUpdate = await useCase.CanUpdateAsync(dto, ct);
        dto.CanDelete = await useCase.CanDeleteAsync(dto, ct);
    }
}