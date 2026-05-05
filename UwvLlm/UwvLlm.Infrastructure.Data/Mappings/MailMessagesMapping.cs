using gAPI.Core.Server.Extensions;
using UwvLlm.Infrastructure.Data.Entities;

namespace UwvLlm.Infrastructure.Data.Mappings;

public class MailMessagesMapping(
    gAPI.Core.Interfaces.IUseCase<MailMessage, Shared.Dtos.MailMessage, Guid> useCase) 
    : gAPI.Core.Interfaces.Mapping<MailMessage, Shared.Dtos.MailMessage>
{
    public override MailMessage ToEntity(
        Shared.Dtos.MailMessage dto, 
        MailMessage entity)
    {
        entity.Id = dto.Id;
        entity.FromUserId = dto.FromUserId;
        entity.ToUserId = dto.ToUserId;
        entity.Subject = dto.Subject;
        entity.Date = dto.Date;
        entity.Body = dto.Body;

        return entity;
    }

    public override async Task<Shared.Dtos.MailMessage> ToDtoAsync(
        MailMessage entity, 
        Shared.Dtos.MailMessage dto,
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

    public override IAsyncEnumerable<Shared.Dtos.MailMessage> ProjectToDtosAsync(
        IQueryable<MailMessage> entities,
        string[]? orderby, 
        int? skip, 
        int? take,
        CancellationToken ct)
    {  
        var dtos = entities
            .Select(entity => new Shared.Dtos.MailMessage()
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
        Shared.Dtos.MailMessage dto,
        CancellationToken ct)
    {
        dto.CanUpdate = await useCase.CanUpdateAsync(dto, ct);
        dto.CanDelete = await useCase.CanDeleteAsync(dto, ct);
    }
}