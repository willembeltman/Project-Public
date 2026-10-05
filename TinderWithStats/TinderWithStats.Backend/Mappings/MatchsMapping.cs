using gAPI.Core.Server.Extensions;

namespace TinderWithStats.Backend.Mappings;

public class MatchsMapping(
    gAPI.Core.Interfaces.IUseCase<TinderWithStats.Backend.Entities.Match, TinderWithStats.Shared.Dtos.Match, Guid> useCase) 
    : gAPI.Core.Interfaces.Mapping<TinderWithStats.Backend.Entities.Match, TinderWithStats.Shared.Dtos.Match>
{
    public override TinderWithStats.Backend.Entities.Match ToEntity(
        TinderWithStats.Shared.Dtos.Match dto, 
        TinderWithStats.Backend.Entities.Match entity)
    {
        entity.Id = dto.Id;
        entity.ProfileSenderId = dto.ProfileSenderId;
        entity.ProfileReceiverId = dto.ProfileReceiverId;
        entity.MatchCreated = dto.MatchCreated;
        entity.MatchAccepted = dto.MatchAccepted;

        return entity;
    }

    public override async Task<TinderWithStats.Shared.Dtos.Match> ToDtoAsync(
        TinderWithStats.Backend.Entities.Match entity, 
        TinderWithStats.Shared.Dtos.Match dto,
        CancellationToken ct)
    {
        dto.Id = entity.Id;
        dto.ProfileSenderId = entity.ProfileSenderId;
        dto.ProfileReceiverId = entity.ProfileReceiverId;
        dto.MatchCreated = entity.MatchCreated;
        dto.MatchAccepted = entity.MatchAccepted;
        
        dto.ProfileSenderName = 
            ("" + (entity?.ProfileSender?.Name ?? default) + "") + " " + 
                (" (" + ((entity?.ProfileSender?.DateOfBirth ?? default).Year) + "-" + ("0" + (entity?.ProfileSender?.DateOfBirth ?? default).Month).Substring(("0" + (entity?.ProfileSender?.DateOfBirth ?? default).Month).Length - 2) + "-" + ("0" + (entity?.ProfileSender?.DateOfBirth ?? default).Day).Substring(("0" + (entity?.ProfileSender?.DateOfBirth ?? default).Day).Length - 2) + ")");

        dto.ProfileReceiverName = 
            ("" + (entity?.ProfileReceiver?.Name ?? default) + "") + " " + 
                (" (" + ((entity?.ProfileReceiver?.DateOfBirth ?? default).Year) + "-" + ("0" + (entity?.ProfileReceiver?.DateOfBirth ?? default).Month).Substring(("0" + (entity?.ProfileReceiver?.DateOfBirth ?? default).Month).Length - 2) + "-" + ("0" + (entity?.ProfileReceiver?.DateOfBirth ?? default).Day).Substring(("0" + (entity?.ProfileReceiver?.DateOfBirth ?? default).Day).Length - 2) + ")");

        await ExtendDto(dto, ct);

        return dto;
    }

    public override IAsyncEnumerable<TinderWithStats.Shared.Dtos.Match> ProjectToDtosAsync(
        IQueryable<TinderWithStats.Backend.Entities.Match> entities,
        string[]? orderby, 
        int? skip, 
        int? take,
        CancellationToken ct)
    {  
        var dtos = entities
            .Select(entity => new TinderWithStats.Shared.Dtos.Match()
            {
                Id = entity.Id,
                ProfileSenderId = entity.ProfileSenderId,
                ProfileReceiverId = entity.ProfileReceiverId,
                MatchCreated = entity.MatchCreated,
                MatchAccepted = entity.MatchAccepted,
#nullable disable
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
        TinderWithStats.Shared.Dtos.Match dto,
        CancellationToken ct)
    {
        dto.CanUpdate = await useCase.CanUpdateAsync(dto, ct);
        dto.CanDelete = await useCase.CanDeleteAsync(dto, ct);
    }
}