using gAPI.Core.Server.Extensions;

namespace UwvLlm.Api.Core.Mappings;

public class MailMessagesMapping(
    gAPI.Core.Interfaces.IUseCase<UwvLlm.Api.Core.Infrastructure.Data.MailMessage, UwvLlm.Shared.Public.Dtos.MailMessage, Guid> useCase) 
    : gAPI.Core.Interfaces.Mapping<UwvLlm.Api.Core.Infrastructure.Data.MailMessage, UwvLlm.Shared.Public.Dtos.MailMessage>
{
    public override UwvLlm.Api.Core.Infrastructure.Data.MailMessage ToEntity(
        UwvLlm.Shared.Public.Dtos.MailMessage dto, 
        UwvLlm.Api.Core.Infrastructure.Data.MailMessage entity)
    {
        entity.Id = dto.Id;
        entity.FromUserId = dto.FromUserId;
        entity.ToUserId = dto.ToUserId;
        entity.Subject = dto.Subject;
        entity.Date = dto.Date;
        entity.Body = dto.Body;

        return entity;
    }

    public override async Task<UwvLlm.Shared.Public.Dtos.MailMessage> ToDtoAsync(
        UwvLlm.Api.Core.Infrastructure.Data.MailMessage entity, 
        UwvLlm.Shared.Public.Dtos.MailMessage dto,
        CancellationToken ct)
    {
        dto.Id = entity.Id;
        dto.FromUserId = entity.FromUserId;
        dto.ToUserId = entity.ToUserId;
        dto.Subject = entity.Subject;
        dto.Date = entity.Date;
        dto.Body = entity.Body;
        
        dto.FromUserName = 
            ("" + (entity?.FromUser?.UserName ?? default) + "");

        dto.ToUserName = 
            ("" + (entity?.ToUser?.UserName ?? default) + "");

        await ExtendDto(dto, ct);

        return dto;
    }

    public override IAsyncEnumerable<UwvLlm.Shared.Public.Dtos.MailMessage> ProjectToDtosAsync(
        IQueryable<UwvLlm.Api.Core.Infrastructure.Data.MailMessage> entities,
        string[]? orderby, 
        int? skip, 
        int? take,
        CancellationToken ct)
    {  
        var dtos = entities
            .Select(entity => new UwvLlm.Shared.Public.Dtos.MailMessage()
            {
                Id = entity.Id,
                FromUserId = entity.FromUserId,
                ToUserId = entity.ToUserId,
                Subject = entity.Subject,
                Date = entity.Date,
                Body = entity.Body,
#nullable disable
                FromUserName = 
                    ("" + entity.FromUser.UserName + ""),
                ToUserName = 
                    ("" + entity.ToUser.UserName + ""),
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
        UwvLlm.Shared.Public.Dtos.MailMessage dto,
        CancellationToken ct)
    {
        dto.CanUpdate = await useCase.CanUpdateAsync(dto, ct);
        dto.CanDelete = await useCase.CanDeleteAsync(dto, ct);
    }
}