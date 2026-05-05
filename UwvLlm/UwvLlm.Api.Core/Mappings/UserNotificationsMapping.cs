using gAPI.Core.Server.Extensions;

namespace UwvLlm.Api.Core.Mappings;

public class UserNotificationsMapping(
    gAPI.Core.Interfaces.IUseCase<UwvLlm.Api.Core.Infrastructure.Data.UserNotification, UwvLlm.Shared.Dtos.UserNotification, long> useCase) 
    : gAPI.Core.Interfaces.Mapping<UwvLlm.Api.Core.Infrastructure.Data.UserNotification, UwvLlm.Shared.Dtos.UserNotification>
{
    public override UwvLlm.Api.Core.Infrastructure.Data.UserNotification ToEntity(
        UwvLlm.Shared.Dtos.UserNotification dto, 
        UwvLlm.Api.Core.Infrastructure.Data.UserNotification entity)
    {
        entity.Id = dto.Id;
        entity.UserId = dto.UserId;
        entity.ExternalType = dto.ExternalType;
        entity.ExternalId = dto.ExternalId;
        entity.Title = dto.Title;
        entity.Message = dto.Message;

        return entity;
    }

    public override async Task<UwvLlm.Shared.Dtos.UserNotification> ToDtoAsync(
        UwvLlm.Api.Core.Infrastructure.Data.UserNotification entity, 
        UwvLlm.Shared.Dtos.UserNotification dto,
        CancellationToken ct)
    {
        dto.Id = entity.Id;
        dto.UserId = entity.UserId;
        dto.ExternalType = entity.ExternalType;
        dto.ExternalId = entity.ExternalId;
        dto.Title = entity.Title;
        dto.Message = entity.Message;
        
        dto.UserName = 
            ("" + (entity?.User?.UserName ?? default) + "");

        await ExtendDto(dto, ct);

        return dto;
    }

    public override IAsyncEnumerable<UwvLlm.Shared.Dtos.UserNotification> ProjectToDtosAsync(
        IQueryable<UwvLlm.Api.Core.Infrastructure.Data.UserNotification> entities,
        string[]? orderby, 
        int? skip, 
        int? take,
        CancellationToken ct)
    {  
        var dtos = entities
            .Select(entity => new UwvLlm.Shared.Dtos.UserNotification()
            {
                Id = entity.Id,
                UserId = entity.UserId,
                ExternalType = entity.ExternalType,
                ExternalId = entity.ExternalId,
                Title = entity.Title,
                Message = entity.Message,
#nullable disable
                UserName = 
                    ("" + entity.User.UserName + ""),
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
        UwvLlm.Shared.Dtos.UserNotification dto,
        CancellationToken ct)
    {
        dto.CanUpdate = await useCase.CanUpdateAsync(dto, ct);
        dto.CanDelete = await useCase.CanDeleteAsync(dto, ct);
    }
}