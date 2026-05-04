using gAPI.Extensions;

namespace UwvLlm.Core.Mappings;

public class MailMessagesMapping(
    gAPI.Interfaces.IUseCase<UwvLlm.Core.Infrastructure.Data.MailMessage, UwvLlm.Shared.Dtos.MailMessage, Guid> useCase) 
    : gAPI.Interfaces.Mapping<UwvLlm.Core.Infrastructure.Data.MailMessage, UwvLlm.Shared.Dtos.MailMessage>
{
    public override UwvLlm.Core.Infrastructure.Data.MailMessage ToEntity(
        UwvLlm.Shared.Dtos.MailMessage dto, 
        UwvLlm.Core.Infrastructure.Data.MailMessage entity)
    {
        entity.Id = dto.Id;
        entity.FromUserId = dto.FromUserId;
        entity.Subject = dto.Subject;
        entity.Date = dto.Date;
        entity.Body = dto.Body;

        return entity;
    }

    public override async Task<UwvLlm.Shared.Dtos.MailMessage> ToDtoAsync(
        UwvLlm.Core.Infrastructure.Data.MailMessage entity, 
        UwvLlm.Shared.Dtos.MailMessage dto,
        CancellationToken ct)
    {
        dto.Id = entity.Id;
        dto.FromUserId = entity.FromUserId;
        dto.Subject = entity.Subject;
        dto.Date = entity.Date;
        dto.Body = entity.Body;
        
        dto.FromUserName = 
            ("" + (entity?.FromUser?.UserName ?? default) + "");

        await ExtendDto(dto, ct);

        return dto;
    }

    public override IAsyncEnumerable<UwvLlm.Shared.Dtos.MailMessage> ProjectToDtosAsync(
        IQueryable<UwvLlm.Core.Infrastructure.Data.MailMessage> entities,
        string[]? orderby, 
        int? skip, 
        int? take,
        CancellationToken ct)
    {  
        var dtos = entities
            .Select(entity => new UwvLlm.Shared.Dtos.MailMessage()
            {
                Id = entity.Id,
                FromUserId = entity.FromUserId,
                Subject = entity.Subject,
                Date = entity.Date,
                Body = entity.Body,
#nullable disable
                FromUserName = 
                    ("" + entity.FromUser.UserName + ""),
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
        UwvLlm.Shared.Dtos.MailMessage dto,
        CancellationToken ct)
    {
        dto.CanUpdate = await useCase.CanUpdateAsync(dto, ct);
        dto.CanDelete = await useCase.CanDeleteAsync(dto, ct);
    }
}