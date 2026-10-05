using gAPI.Core.Dtos;
using gAPI.Core.Enums;
using TinderWithStats.Shared.Dtos;
using TinderWithStats.Shared.Interfaces;

namespace TinderWithStats.Backend.Services;

public class ChatMessagesApi(
    gAPI.Core.Interfaces.IUseCase<TinderWithStats.Backend.Entities.ChatMessage, ChatMessage, Guid> useCase,
    gAPI.Core.Interfaces.Mapping<TinderWithStats.Backend.Entities.ChatMessage, ChatMessage> mapping)
    : IChatMessagesApi
{
    public async Task<BaseResponseT<ChatMessage>> Create(ChatMessage dto, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByMatchAsync(dto, ct);

        if (entity != null)
            return new BaseResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorAlreadyUsed };

        entity = mapping.ToEntity(dto, new TinderWithStats.Backend.Entities.ChatMessage());

        if (!await useCase.CanCreateAsync(dto, ct))
            return new BaseResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.AddAsync(entity, ct))
            return new BaseResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorAttachingState };

        dto = await mapping.ToDtoAsync(entity, new ChatMessage(), ct);

        return new BaseResponseT<ChatMessage>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<ChatMessage>> Read(Guid chatmessageId, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(chatmessageId, ct);
        if (entity == null)
            return new BaseResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        var dto = await mapping.ToDtoAsync(entity, new ChatMessage(), ct);

        if (!await useCase.CanReadAsync(dto, ct))
            return new BaseResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        return new BaseResponseT<ChatMessage>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<ChatMessage>> Update(ChatMessage dto, CancellationToken ct)
    {
        if (dto == null)
            return new BaseResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorItemNotSupplied };

        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(dto.Id, ct);
        if (entity == null)
            return new BaseResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        if (!await useCase.CanUpdateAsync(dto, ct))
            return new BaseResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        mapping.ToEntity(dto, entity);

        if (!await useCase.UpdateAsync(entity, dto, ct))
            return new BaseResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorUpdatingState };

        dto = await mapping.ToDtoAsync(entity, dto, ct);

        return new BaseResponseT<ChatMessage>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<bool>> Delete(Guid chatmessageId, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(chatmessageId, ct);
        if (entity == null)
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        var dto = await mapping.ToDtoAsync(entity, new ChatMessage(), ct);

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

    public async Task<BaseListResponseT<ChatMessage>> List(int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase.ListAll();

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<ChatMessage>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = await dtos.ToArrayAsync(ct)
        };
    }

    public async Task<BaseListResponseT<ChatMessage>> ListByMatchId(Guid matchId, int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase
            .ListAll()
            .Where(a => a.MatchId == matchId);

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<ChatMessage>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = await dtos.ToArrayAsync(ct)
        };
    }

    public async Task<BaseListResponseT<ChatMessage>> ListNotByMatchId(Guid matchId, int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase
            .ListAll()
            .Where(a => a.MatchId != matchId);

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<ChatMessage>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = await dtos.ToArrayAsync(ct)
        };
    }

    public async Task<BaseListResponseT<ChatMessage>> ListByProfileSenderId(Guid profileSenderId, int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase
            .ListAll()
            .Where(a => a.ProfileSenderId == profileSenderId);

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<ChatMessage>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = await dtos.ToArrayAsync(ct)
        };
    }

    public async Task<BaseListResponseT<ChatMessage>> ListNotByProfileSenderId(Guid profileSenderId, int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase
            .ListAll()
            .Where(a => a.ProfileSenderId != profileSenderId);

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<ChatMessage>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = await dtos.ToArrayAsync(ct)
        };
    }

    public async Task<BaseListResponseT<ChatMessage>> ListByProfileReceiverId(Guid profileReceiverId, int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase
            .ListAll()
            .Where(a => a.ProfileReceiverId == profileReceiverId);

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<ChatMessage>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = await dtos.ToArrayAsync(ct)
        };
    }

    public async Task<BaseListResponseT<ChatMessage>> ListNotByProfileReceiverId(Guid profileReceiverId, int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase
            .ListAll()
            .Where(a => a.ProfileReceiverId != profileReceiverId);

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<ChatMessage>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<ChatMessage>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = await dtos.ToArrayAsync(ct)
        };
    }
}