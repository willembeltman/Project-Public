using gAPI.Dtos;
using gAPI.Enums;
using gAPI.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.Core.CrudServices;

public class UsersService(
    gAPI.Interfaces.IUseCase<Infrastructure.Data.User, User, Guid> useCase,
    gAPI.Interfaces.Mapping<Infrastructure.Data.User, User> mapping,
    IStorageService storageService)
    : IUsersService
{
    public async Task<BaseResponseT<User>> Create(User dto, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<User>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByMatchAsync(dto, ct);

        if (entity != null)
            return new BaseResponseT<User>() { Error = BaseResponseErrorEnum.ErrorAlreadyUsed };

        entity = mapping.ToEntity(dto, new Infrastructure.Data.User());

        if (!await useCase.CanCreateAsync(dto, ct))
            return new BaseResponseT<User>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.AddAsync(entity, ct))
            return new BaseResponseT<User>() { Error = BaseResponseErrorEnum.ErrorAttachingState };

        dto = await mapping.ToDtoAsync(entity, new User(), ct);

        return new BaseResponseT<User>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<User>> Read(Guid userId, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<User>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(userId, ct);
        if (entity == null)
            return new BaseResponseT<User>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        var dto = await mapping.ToDtoAsync(entity, new User(), ct);

        if (!await useCase.CanReadAsync(dto, ct))
            return new BaseResponseT<User>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        return new BaseResponseT<User>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<User>> Update(User dto, CancellationToken ct)
    {
        if (dto == null)
            return new BaseResponseT<User>() { Error = BaseResponseErrorEnum.ErrorItemNotSupplied };

        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<User>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(dto.Id, ct);
        if (entity == null)
            return new BaseResponseT<User>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        if (!await useCase.CanUpdateAsync(dto, ct))
            return new BaseResponseT<User>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        mapping.ToEntity(dto, entity);

        if (!await useCase.UpdateAsync(entity, dto, ct))
            return new BaseResponseT<User>() { Error = BaseResponseErrorEnum.ErrorUpdatingState };

        dto = await mapping.ToDtoAsync(entity, dto, ct);

        return new BaseResponseT<User>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<bool>> Delete(Guid userId, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(userId, ct);
        if (entity == null)
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        var dto = await mapping.ToDtoAsync(entity, new User(), ct);

        if (!await useCase.CanDeleteAsync(dto, ct))
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        await storageService.DeleteStorageFileAsync(entity, ct);

        if (!await useCase.RemoveAsync(entity, ct))
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorUpdatingState };

        return new BaseResponseT<bool>() 
        { 
            Success = true,
            Response = true 
        };
    }

    public async Task<BaseListResponseT<User>> List(int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<User>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<User>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase.ListAll();

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<User>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<User>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = dtos
        };
    }

    public async Task<BaseResponseT<User>> FileUpdate(Guid userId, IFormFile? file, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<User>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };
        
        var entity = await useCase.FindByIdAsync(userId, ct);
        if (entity == null)
            return new BaseResponseT<User>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        var dto = await mapping.ToDtoAsync(entity, new User(), ct);

        if (!await useCase.CanUpdateAsync(dto, ct))
            return new BaseResponseT<User>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (file != null)
        {
            using var storageFileStream = file.OpenReadStream();
            await storageService.SaveStorageFileAsync(entity, file.FileName, file.ContentType, storageFileStream, ct);
        }

        dto = await mapping.ToDtoAsync(entity, new User(), ct);

        if (!await useCase.UpdateAsync(entity, dto, ct))
            return new BaseResponseT<User>() { Error = BaseResponseErrorEnum.ErrorUpdatingState };

        return new BaseResponseT<User>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<bool>> FileDelete(Guid userId, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(userId, ct);

        if (entity == null)
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        var dto = await mapping.ToDtoAsync(entity, new User(), ct);

        if (!await useCase.CanDeleteAsync(dto, ct))
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        await storageService.DeleteStorageFileAsync(entity, ct);

        dto = await mapping.ToDtoAsync(entity, new User(), ct);

        if (!await useCase.UpdateAsync(entity, dto, ct))
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorUpdatingState };

        return new BaseResponseT<bool>() 
        { 
            Success = true,
            Response = true 
        };
    }
}