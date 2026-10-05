using gAPI.Core.Server.Extensions;

namespace TinderWithStats.Backend.Mappings;

public class ChatMessagesMapping(
    gAPI.Core.Interfaces.IUseCase<TinderWithStats.Backend.Entities.ChatMessage, TinderWithStats.Shared.Dtos.ChatMessage, Guid> useCase) 
    : gAPI.Core.Interfaces.Mapping<TinderWithStats.Backend.Entities.ChatMessage, TinderWithStats.Shared.Dtos.ChatMessage>
{
    public override TinderWithStats.Backend.Entities.ChatMessage ToEntity(
        TinderWithStats.Shared.Dtos.ChatMessage dto, 
        TinderWithStats.Backend.Entities.ChatMessage entity)
    {
        entity.Id = dto.Id;
        entity.MatchId = dto.MatchId;
        entity.ProfileSenderId = dto.ProfileSenderId;
        entity.ProfileReceiverId = dto.ProfileReceiverId;
        entity.Date = dto.Date;
        entity.Message = dto.Message;

        return entity;
    }

    public override async Task<TinderWithStats.Shared.Dtos.ChatMessage> ToDtoAsync(
        TinderWithStats.Backend.Entities.ChatMessage entity, 
        TinderWithStats.Shared.Dtos.ChatMessage dto,
        CancellationToken ct)
    {
        dto.Id = entity.Id;
        dto.MatchId = entity.MatchId;
        dto.ProfileSenderId = entity.ProfileSenderId;
        dto.ProfileReceiverId = entity.ProfileReceiverId;
        dto.Date = entity.Date;
        dto.Message = entity.Message;
        
        dto.MatchName = 
            ("" + ((entity?.Match?.MatchCreated ?? default).Year) + "-" + ("0" + (entity?.Match?.MatchCreated ?? default).Month).Substring(("0" + (entity?.Match?.MatchCreated ?? default).Month).Length - 2) + "-" + ("0" + (entity?.Match?.MatchCreated ?? default).Day).Substring(("0" + (entity?.Match?.MatchCreated ?? default).Day).Length - 2) + "") + " " + 
                ("" + ((entity?.Match?.MatchAccepted ?? default).Year) + "-" + ("0" + (entity?.Match?.MatchAccepted ?? default).Month).Substring(("0" + (entity?.Match?.MatchAccepted ?? default).Month).Length - 2) + "-" + ("0" + (entity?.Match?.MatchAccepted ?? default).Day).Substring(("0" + (entity?.Match?.MatchAccepted ?? default).Day).Length - 2) + "");

        dto.ProfileSenderName = 
            ("" + (entity?.ProfileSender?.Name ?? default) + "") + " " + 
                (" (" + ((entity?.ProfileSender?.DateOfBirth ?? default).Year) + "-" + ("0" + (entity?.ProfileSender?.DateOfBirth ?? default).Month).Substring(("0" + (entity?.ProfileSender?.DateOfBirth ?? default).Month).Length - 2) + "-" + ("0" + (entity?.ProfileSender?.DateOfBirth ?? default).Day).Substring(("0" + (entity?.ProfileSender?.DateOfBirth ?? default).Day).Length - 2) + ")");

        dto.ProfileReceiverName = 
            ("" + (entity?.ProfileReceiver?.Name ?? default) + "") + " " + 
                (" (" + ((entity?.ProfileReceiver?.DateOfBirth ?? default).Year) + "-" + ("0" + (entity?.ProfileReceiver?.DateOfBirth ?? default).Month).Substring(("0" + (entity?.ProfileReceiver?.DateOfBirth ?? default).Month).Length - 2) + "-" + ("0" + (entity?.ProfileReceiver?.DateOfBirth ?? default).Day).Substring(("0" + (entity?.ProfileReceiver?.DateOfBirth ?? default).Day).Length - 2) + ")");

        await ExtendDto(dto, ct);

        return dto;
    }

    public override IAsyncEnumerable<TinderWithStats.Shared.Dtos.ChatMessage> ProjectToDtosAsync(
        IQueryable<TinderWithStats.Backend.Entities.ChatMessage> entities,
        string[]? orderby, 
        int? skip, 
        int? take,
        CancellationToken ct)
    {  
        var dtos = entities
            .Select(entity => new TinderWithStats.Shared.Dtos.ChatMessage()
            {
                Id = entity.Id,
                MatchId = entity.MatchId,
                ProfileSenderId = entity.ProfileSenderId,
                ProfileReceiverId = entity.ProfileReceiverId,
                Date = entity.Date,
                Message = entity.Message,
#nullable disable
                MatchName = 
                    ("" + (entity.Match.MatchCreated.Year) + "-" + ("0" + entity.Match.MatchCreated.Month).Substring(("0" + entity.Match.MatchCreated.Month).Length - 2) + "-" + ("0" + entity.Match.MatchCreated.Day).Substring(("0" + entity.Match.MatchCreated.Day).Length - 2) + "") + " " + 
                        ("" + (entity.Match.MatchAccepted.Value.Year) + "-" + ("0" + entity.Match.MatchAccepted.Value.Month).Substring(("0" + entity.Match.MatchAccepted.Value.Month).Length - 2) + "-" + ("0" + entity.Match.MatchAccepted.Value.Day).Substring(("0" + entity.Match.MatchAccepted.Value.Day).Length - 2) + ""),
                ProfileSenderName = 
                    ("" + entity.ProfileSender.Name + "") + " " + 
                        (" (" + (entity.ProfileSender.DateOfBirth.Year) + "-" + ("0" + entity.ProfileSender.DateOfBirth.Month).Substring(("0" + entity.ProfileSender.DateOfBirth.Month).Length - 2) + "-" + ("0" + entity.ProfileSender.DateOfBirth.Day).Substring(("0" + entity.ProfileSender.DateOfBirth.Day).Length - 2) + ")"),
                ProfileReceiverName = 
                    ("" + entity.ProfileReceiver.Name + "") + " " + 
                        (" (" + (entity.ProfileReceiver.DateOfBirth.Year) + "-" + ("0" + entity.ProfileReceiver.DateOfBirth.Month).Substring(("0" + entity.ProfileReceiver.DateOfBirth.Month).Length - 2) + "-" + ("0" + entity.ProfileReceiver.DateOfBirth.Day).Substring(("0" + entity.ProfileReceiver.DateOfBirth.Day).Length - 2) + ")"),
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
        TinderWithStats.Shared.Dtos.ChatMessage dto,
        CancellationToken ct)
    {
        dto.CanUpdate = await useCase.CanUpdateAsync(dto, ct);
        dto.CanDelete = await useCase.CanDeleteAsync(dto, ct);
    }
}