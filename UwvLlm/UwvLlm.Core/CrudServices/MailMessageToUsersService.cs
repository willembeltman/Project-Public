using gAPI.Dtos;
using gAPI.Enums;
using Microsoft.EntityFrameworkCore;
using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.Core.CrudServices;

public class MailMessageToUsersService(
    gAPI.Interfaces.IUseCase<UwvLlm.Core.Infrastructure.Data.MailMessageToUser, MailMessageToUser, long> useCase,
    gAPI.Interfaces.Mapping<UwvLlm.Core.Infrastructure.Data.MailMessageToUser, MailMessageToUser> mapping)
    : IMailMessageToUsersService
{
    public async Task<BaseResponseT<MailMessageToUser>> Create(MailMessageToUser dto, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByMatchAsync(dto, ct);

        if (entity != null)
            return new BaseResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorAlreadyUsed };

        entity = mapping.ToEntity(dto, new UwvLlm.Core.Infrastructure.Data.MailMessageToUser());

        if (!await useCase.CanCreateAsync(dto, ct))
            return new BaseResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.AddAsync(entity, ct))
            return new BaseResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorAttachingState };

        dto = await mapping.ToDtoAsync(entity, new MailMessageToUser(), ct);

        return new BaseResponseT<MailMessageToUser>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<MailMessageToUser>> Read(long mailmessagetouserId, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(mailmessagetouserId, ct);
        if (entity == null)
            return new BaseResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        var dto = await mapping.ToDtoAsync(entity, new MailMessageToUser(), ct);

        if (!await useCase.CanReadAsync(dto, ct))
            return new BaseResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        return new BaseResponseT<MailMessageToUser>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<MailMessageToUser>> Update(MailMessageToUser dto, CancellationToken ct)
    {
        if (dto == null)
            return new BaseResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorItemNotSupplied };

        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(dto.Id, ct);
        if (entity == null)
            return new BaseResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        if (!await useCase.CanUpdateAsync(dto, ct))
            return new BaseResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        mapping.ToEntity(dto, entity);

        if (!await useCase.UpdateAsync(entity, dto, ct))
            return new BaseResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorUpdatingState };

        dto = await mapping.ToDtoAsync(entity, dto, ct);

        return new BaseResponseT<MailMessageToUser>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<bool>> Delete(long mailmessagetouserId, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(mailmessagetouserId, ct);
        if (entity == null)
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        var dto = await mapping.ToDtoAsync(entity, new MailMessageToUser(), ct);

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

    public async Task<BaseListResponseT<MailMessageToUser>> List(int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase.ListAll();

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<MailMessageToUser>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = dtos
        };
    }

    public async Task<BaseListResponseT<MailMessageToUser>> ListByMailMessageId(Guid mailMessageId, int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase
            .ListAll()
            .Where(a => a.MailMessageId == mailMessageId);

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<MailMessageToUser>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = dtos
        };
    }

    public async Task<BaseListResponseT<MailMessageToUser>> ListNotByMailMessageId(Guid mailMessageId, int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase
            .ListAll()
            .Where(a => a.MailMessageId != mailMessageId);

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<MailMessageToUser>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = dtos
        };
    }

    public async Task<BaseListResponseT<MailMessageToUser>> ListByUserId(Guid userId, int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase
            .ListAll()
            .Where(a => a.UserId == userId);

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<MailMessageToUser>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = dtos
        };
    }

    public async Task<BaseListResponseT<MailMessageToUser>> ListNotByUserId(Guid userId, int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase
            .ListAll()
            .Where(a => a.UserId != userId);

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<MailMessageToUser>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<MailMessageToUser>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = dtos
        };
    }
}