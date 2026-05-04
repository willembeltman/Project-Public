using gAPI.Dtos;
using gAPI.Enums;
using Microsoft.EntityFrameworkCore;
using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.Core.CrudServices;

public class UserNotificationQuickOptionsService(
    gAPI.Interfaces.IUseCase<Infrastructure.Data.UserNotificationQuickOption, UserNotificationQuickOption, long> useCase,
    gAPI.Interfaces.Mapping<Infrastructure.Data.UserNotificationQuickOption, UserNotificationQuickOption> mapping)
    : IUserNotificationQuickOptionsService
{
    public async Task<BaseResponseT<UserNotificationQuickOption>> Create(UserNotificationQuickOption dto, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<UserNotificationQuickOption>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByMatchAsync(dto, ct);

        if (entity != null)
            return new BaseResponseT<UserNotificationQuickOption>() { Error = BaseResponseErrorEnum.ErrorAlreadyUsed };

        entity = mapping.ToEntity(dto, new Infrastructure.Data.UserNotificationQuickOption());

        if (!await useCase.CanCreateAsync(dto, ct))
            return new BaseResponseT<UserNotificationQuickOption>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.AddAsync(entity, ct))
            return new BaseResponseT<UserNotificationQuickOption>() { Error = BaseResponseErrorEnum.ErrorAttachingState };

        dto = await mapping.ToDtoAsync(entity, new UserNotificationQuickOption(), ct);

        return new BaseResponseT<UserNotificationQuickOption>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<UserNotificationQuickOption>> Read(long usernotificationquickoptionId, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<UserNotificationQuickOption>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(usernotificationquickoptionId, ct);
        if (entity == null)
            return new BaseResponseT<UserNotificationQuickOption>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        var dto = await mapping.ToDtoAsync(entity, new UserNotificationQuickOption(), ct);

        if (!await useCase.CanReadAsync(dto, ct))
            return new BaseResponseT<UserNotificationQuickOption>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        return new BaseResponseT<UserNotificationQuickOption>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<UserNotificationQuickOption>> Update(UserNotificationQuickOption dto, CancellationToken ct)
    {
        if (dto == null)
            return new BaseResponseT<UserNotificationQuickOption>() { Error = BaseResponseErrorEnum.ErrorItemNotSupplied };

        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<UserNotificationQuickOption>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(dto.Id, ct);
        if (entity == null)
            return new BaseResponseT<UserNotificationQuickOption>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        if (!await useCase.CanUpdateAsync(dto, ct))
            return new BaseResponseT<UserNotificationQuickOption>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        mapping.ToEntity(dto, entity);

        if (!await useCase.UpdateAsync(entity, dto, ct))
            return new BaseResponseT<UserNotificationQuickOption>() { Error = BaseResponseErrorEnum.ErrorUpdatingState };

        dto = await mapping.ToDtoAsync(entity, dto, ct);

        return new BaseResponseT<UserNotificationQuickOption>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<bool>> Delete(long usernotificationquickoptionId, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(usernotificationquickoptionId, ct);
        if (entity == null)
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        var dto = await mapping.ToDtoAsync(entity, new UserNotificationQuickOption(), ct);

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

    public async Task<BaseListResponseT<UserNotificationQuickOption>> List(int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<UserNotificationQuickOption>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<UserNotificationQuickOption>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase.ListAll();

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<UserNotificationQuickOption>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<UserNotificationQuickOption>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = dtos
        };
    }

    public async Task<BaseListResponseT<UserNotificationQuickOption>> ListByUserNotificationId(long userNotificationId, int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<UserNotificationQuickOption>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<UserNotificationQuickOption>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase
            .ListAll()
            .Where(a => a.UserNotificationId == userNotificationId);

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<UserNotificationQuickOption>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<UserNotificationQuickOption>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = dtos
        };
    }

    public async Task<BaseListResponseT<UserNotificationQuickOption>> ListNotByUserNotificationId(long userNotificationId, int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<UserNotificationQuickOption>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<UserNotificationQuickOption>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase
            .ListAll()
            .Where(a => a.UserNotificationId != userNotificationId);

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<UserNotificationQuickOption>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<UserNotificationQuickOption>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = dtos
        };
    }
}