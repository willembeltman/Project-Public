using gAPI.Extensions;

namespace UwvLlm.Core.Mappings;

public class MailMessageToUsersMapping(
    gAPI.Interfaces.IUseCase<UwvLlm.Core.Infrastructure.Data.MailMessageToUser, UwvLlm.Shared.Dtos.MailMessageToUser, long> useCase) 
    : gAPI.Interfaces.Mapping<UwvLlm.Core.Infrastructure.Data.MailMessageToUser, UwvLlm.Shared.Dtos.MailMessageToUser>
{
    public override UwvLlm.Core.Infrastructure.Data.MailMessageToUser ToEntity(
        UwvLlm.Shared.Dtos.MailMessageToUser dto, 
        UwvLlm.Core.Infrastructure.Data.MailMessageToUser entity)
    {
        entity.Id = dto.Id;
        entity.MailMessageId = dto.MailMessageId;
        entity.UserId = dto.UserId;

        return entity;
    }

    public override async Task<UwvLlm.Shared.Dtos.MailMessageToUser> ToDtoAsync(
        UwvLlm.Core.Infrastructure.Data.MailMessageToUser entity, 
        UwvLlm.Shared.Dtos.MailMessageToUser dto,
        CancellationToken ct)
    {
        dto.Id = entity.Id;
        dto.MailMessageId = entity.MailMessageId;
        dto.UserId = entity.UserId;
        
        dto.MailMessageName = 
            ("" + (entity?.MailMessage?.Subject ?? default) + "") + " " + 
                ("(" + ("0" + (entity?.MailMessage?.Date ?? default).Day).Substring(("0" + (entity?.MailMessage?.Date ?? default).Day).Length - 2) + "-" + ("0" + (entity?.MailMessage?.Date ?? default).Month).Substring(("0" + (entity?.MailMessage?.Date ?? default).Month).Length - 2) + "-" + ((entity?.MailMessage?.Date ?? default).Year) + " " + ("0" + (entity?.MailMessage?.Date ?? default).Hour).Substring(("0" + (entity?.MailMessage?.Date ?? default).Hour).Length - 2) + ":" + ("0" + (entity?.MailMessage?.Date ?? default).Minute).Substring(("0" + (entity?.MailMessage?.Date ?? default).Minute).Length - 2) + ")");

        dto.UserName = 
            ("" + (entity?.User?.UserName ?? default) + "");

        await ExtendDto(dto, ct);

        return dto;
    }

    public override IAsyncEnumerable<UwvLlm.Shared.Dtos.MailMessageToUser> ProjectToDtosAsync(
        IQueryable<UwvLlm.Core.Infrastructure.Data.MailMessageToUser> entities,
        string[]? orderby, 
        int? skip, 
        int? take,
        CancellationToken ct)
    {  
        var dtos = entities
            .Select(entity => new UwvLlm.Shared.Dtos.MailMessageToUser()
            {
                Id = entity.Id,
                MailMessageId = entity.MailMessageId,
                UserId = entity.UserId,
#nullable disable
                MailMessageName = 
                    ("" + entity.MailMessage.Subject + "") + " " + 
                        ("(" + ("0" + entity.MailMessage.Date.Day).Substring(("0" + entity.MailMessage.Date.Day).Length - 2) + "-" + ("0" + entity.MailMessage.Date.Month).Substring(("0" + entity.MailMessage.Date.Month).Length - 2) + "-" + (entity.MailMessage.Date.Year) + " " + ("0" + entity.MailMessage.Date.Hour).Substring(("0" + entity.MailMessage.Date.Hour).Length - 2) + ":" + ("0" + entity.MailMessage.Date.Minute).Substring(("0" + entity.MailMessage.Date.Minute).Length - 2) + ")"),
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
        UwvLlm.Shared.Dtos.MailMessageToUser dto,
        CancellationToken ct)
    {
        dto.CanUpdate = await useCase.CanUpdateAsync(dto, ct);
        dto.CanDelete = await useCase.CanDeleteAsync(dto, ct);
    }
}