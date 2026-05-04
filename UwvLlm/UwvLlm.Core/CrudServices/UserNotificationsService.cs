using gAPI.Dtos;
using gAPI.Enums;
using Microsoft.EntityFrameworkCore;
using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.Core.CrudServices;

public class UserNotificationsService(
    gAPI.Interfaces.IUseCase<Infrastructure.Data.UserNotification, UserNotification, long> useCase,
    gAPI.Interfaces.Mapping<Infrastructure.Data.UserNotification, UserNotification> mapping)
    : IUserNotificationsService
{
    public async Task<BaseResponseT<UserNotification>> Create(UserNotification dto, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<UserNotification>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByMatchAsync(dto, ct);

        if (entity != null)
            return new BaseResponseT<UserNotification>() { Error = BaseResponseErrorEnum.ErrorAlreadyUsed };

        entity = mapping.ToEntity(dto, new Infrastructure.Data.UserNotification());

        if (!await useCase.CanCreateAsync(dto, ct))
            return new BaseResponseT<UserNotification>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.AddAsync(entity, ct))
            return new BaseResponseT<UserNotification>() { Error = BaseResponseErrorEnum.ErrorAttachingState };

        dto = await mapping.ToDtoAsync(entity, new UserNotification(), ct);

        return new BaseResponseT<UserNotification>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<UserNotification>> Read(long usernotificationId, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<UserNotification>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(usernotificationId, ct);
        if (entity == null)
            return new BaseResponseT<UserNotification>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        var dto = await mapping.ToDtoAsync(entity, new UserNotification(), ct);

        if (!await useCase.CanReadAsync(dto, ct))
            return new BaseResponseT<UserNotification>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        return new BaseResponseT<UserNotification>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<UserNotification>> Update(UserNotification dto, CancellationToken ct)
    {
        if (dto == null)
            return new BaseResponseT<UserNotification>() { Error = BaseResponseErrorEnum.ErrorItemNotSupplied };

        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<UserNotification>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(dto.Id, ct);
        if (entity == null)
            return new BaseResponseT<UserNotification>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        if (!await useCase.CanUpdateAsync(dto, ct))
            return new BaseResponseT<UserNotification>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        mapping.ToEntity(dto, entity);

        if (!await useCase.UpdateAsync(entity, dto, ct))
            return new BaseResponseT<UserNotification>() { Error = BaseResponseErrorEnum.ErrorUpdatingState };

        dto = await mapping.ToDtoAsync(entity, dto, ct);

        return new BaseResponseT<UserNotification>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<bool>> Delete(long usernotificationId, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(usernotificationId, ct);
        if (entity == null)
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        var dto = await mapping.ToDtoAsync(entity, new UserNotification(), ct);

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

    public async Task<BaseListResponseT<UserNotification>> List(int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<UserNotification>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<UserNotification>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase.ListAll();

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<UserNotification>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<UserNotification>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = dtos
        };
    }

    public async Task<BaseListResponseT<UserNotification>> ListByUserId(Guid userId, int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<UserNotification>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<UserNotification>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase
            .ListAll()
            .Where(a => a.UserId == userId);

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<UserNotification>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<UserNotification>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = dtos
        };
    }

    public async Task<BaseListResponseT<UserNotification>> ListNotByUserId(Guid userId, int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<UserNotification>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<UserNotification>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase
            .ListAll()
            .Where(a => a.UserId != userId);

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<UserNotification>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<UserNotification>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = dtos
        };
    }
}