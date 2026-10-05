using gAPI.Core.Dtos;
using gAPI.Core.Enums;
using TinderWithStats.Shared.Dtos;
using TinderWithStats.Shared.Interfaces;

namespace TinderWithStats.Backend.Services;

public class MatchsApi(
    gAPI.Core.Interfaces.IUseCase<TinderWithStats.Backend.Entities.Match, Match, Guid> useCase,
    gAPI.Core.Interfaces.Mapping<TinderWithStats.Backend.Entities.Match, Match> mapping)
    : IMatchsApi
{
    public async Task<BaseResponseT<Match>> Create(Match dto, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByMatchAsync(dto, ct);

        if (entity != null)
            return new BaseResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorAlreadyUsed };

        entity = mapping.ToEntity(dto, new TinderWithStats.Backend.Entities.Match());

        if (!await useCase.CanCreateAsync(dto, ct))
            return new BaseResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.AddAsync(entity, ct))
            return new BaseResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorAttachingState };

        dto = await mapping.ToDtoAsync(entity, new Match(), ct);

        return new BaseResponseT<Match>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<Match>> Read(Guid matchId, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(matchId, ct);
        if (entity == null)
            return new BaseResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        var dto = await mapping.ToDtoAsync(entity, new Match(), ct);

        if (!await useCase.CanReadAsync(dto, ct))
            return new BaseResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        return new BaseResponseT<Match>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<Match>> Update(Match dto, CancellationToken ct)
    {
        if (dto == null)
            return new BaseResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorItemNotSupplied };

        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(dto.Id, ct);
        if (entity == null)
            return new BaseResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        if (!await useCase.CanUpdateAsync(dto, ct))
            return new BaseResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        mapping.ToEntity(dto, entity);

        if (!await useCase.UpdateAsync(entity, dto, ct))
            return new BaseResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorUpdatingState };

        dto = await mapping.ToDtoAsync(entity, dto, ct);

        return new BaseResponseT<Match>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<bool>> Delete(Guid matchId, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(matchId, ct);
        if (entity == null)
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        var dto = await mapping.ToDtoAsync(entity, new Match(), ct);

        if (!await useCase.CanDeleteAsync(dto, ct))
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.RemoveAsync(entity, ct))
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorUpdatingState };

        return new BaseResponseT<bool>() 
        { 
            Success = true,
            Response = true 
        };
    }

    public async Task<BaseListResponseT<Match>> List(int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase.ListAll();

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<Match>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = await dtos.ToArrayAsync(ct)
        };
    }

    public async Task<BaseListResponseT<Match>> ListByProfileSenderId(Guid profileSenderId, int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase
            .ListAll()
            .Where(a => a.ProfileSenderId == profileSenderId);

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<Match>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = await dtos.ToArrayAsync(ct)
        };
    }

    public async Task<BaseListResponseT<Match>> ListNotByProfileSenderId(Guid profileSenderId, int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase
            .ListAll()
            .Where(a => a.ProfileSenderId != profileSenderId);

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<Match>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = await dtos.ToArrayAsync(ct)
        };
    }

    public async Task<BaseListResponseT<Match>> ListByProfileReceiverId(Guid profileReceiverId, int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase
            .ListAll()
            .Where(a => a.ProfileReceiverId == profileReceiverId);

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<Match>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = await dtos.ToArrayAsync(ct)
        };
    }

    public async Task<BaseListResponseT<Match>> ListNotByProfileReceiverId(Guid profileReceiverId, int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase
            .ListAll()
            .Where(a => a.ProfileReceiverId != profileReceiverId);

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<Match>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<Match>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = await dtos.ToArrayAsync(ct)
        };
    }
}