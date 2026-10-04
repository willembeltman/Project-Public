using gAPI.Core.Dtos;
using gAPI.Core.Enums;
using gAPI.Core.Server.Storage;
using Microsoft.AspNetCore.Http;
using TinderWithStats.Shared.Dtos;
using TinderWithStats.Shared.Interfaces;

namespace TinderWithStats.Backend.Services;

public class ProfilePicturesApi(
    gAPI.Core.Interfaces.IUseCase<TinderWithStats.Backend.Entities.ProfilePicture, ProfilePicture, Guid> useCase,
    gAPI.Core.Interfaces.Mapping<TinderWithStats.Backend.Entities.ProfilePicture, ProfilePicture> mapping,
    IStorageService storageService)
    : IProfilePicturesApi
{
    public async Task<BaseResponseT<ProfilePicture>> Create(ProfilePicture dto, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<ProfilePicture>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByMatchAsync(dto, ct);

        if (entity != null)
            return new BaseResponseT<ProfilePicture>() { Error = BaseResponseErrorEnum.ErrorAlreadyUsed };

        entity = mapping.ToEntity(dto, new TinderWithStats.Backend.Entities.ProfilePicture());

        if (!await useCase.CanCreateAsync(dto, ct))
            return new BaseResponseT<ProfilePicture>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.AddAsync(entity, ct))
            return new BaseResponseT<ProfilePicture>() { Error = BaseResponseErrorEnum.ErrorAttachingState };

        dto = await mapping.ToDtoAsync(entity, new ProfilePicture(), ct);

        return new BaseResponseT<ProfilePicture>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<ProfilePicture>> Read(Guid profilepictureId, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<ProfilePicture>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(profilepictureId, ct);
        if (entity == null)
            return new BaseResponseT<ProfilePicture>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        var dto = await mapping.ToDtoAsync(entity, new ProfilePicture(), ct);

        if (!await useCase.CanReadAsync(dto, ct))
            return new BaseResponseT<ProfilePicture>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        return new BaseResponseT<ProfilePicture>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<ProfilePicture>> Update(ProfilePicture dto, CancellationToken ct)
    {
        if (dto == null)
            return new BaseResponseT<ProfilePicture>() { Error = BaseResponseErrorEnum.ErrorItemNotSupplied };

        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<ProfilePicture>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(dto.Id, ct);
        if (entity == null)
            return new BaseResponseT<ProfilePicture>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        if (!await useCase.CanUpdateAsync(dto, ct))
            return new BaseResponseT<ProfilePicture>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        mapping.ToEntity(dto, entity);

        if (!await useCase.UpdateAsync(entity, dto, ct))
            return new BaseResponseT<ProfilePicture>() { Error = BaseResponseErrorEnum.ErrorUpdatingState };

        dto = await mapping.ToDtoAsync(entity, dto, ct);

        return new BaseResponseT<ProfilePicture>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<bool>> Delete(Guid profilepictureId, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(profilepictureId, ct);
        if (entity == null)
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        var dto = await mapping.ToDtoAsync(entity, new ProfilePicture(), ct);

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

    public async Task<BaseListResponseT<ProfilePicture>> List(int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<ProfilePicture>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<ProfilePicture>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase.ListAll();

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<ProfilePicture>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<ProfilePicture>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = await dtos.ToArrayAsync(ct)
        };
    }

    public async Task<BaseListResponseT<ProfilePicture>> ListByProfileId(Guid profileId, int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<ProfilePicture>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<ProfilePicture>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase
            .ListAll()
            .Where(a => a.ProfileId == profileId);

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<ProfilePicture>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<ProfilePicture>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = await dtos.ToArrayAsync(ct)
        };
    }

    public async Task<BaseListResponseT<ProfilePicture>> ListNotByProfileId(Guid profileId, int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<ProfilePicture>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<ProfilePicture>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase
            .ListAll()
            .Where(a => a.ProfileId != profileId);

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<ProfilePicture>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<ProfilePicture>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = await dtos.ToArrayAsync(ct)
        };
    }

    public async Task<BaseResponseT<ProfilePicture>> FileUpdate(Guid profilepictureId, IFormFile? file, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<ProfilePicture>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };
        
        var entity = await useCase.FindByIdAsync(profilepictureId, ct);
        if (entity == null)
            return new BaseResponseT<ProfilePicture>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        var dto = await mapping.ToDtoAsync(entity, new ProfilePicture(), ct);

        if (!await useCase.CanUpdateAsync(dto, ct))
            return new BaseResponseT<ProfilePicture>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (file != null)
        {
            using var storageFileStream = file.OpenReadStream();
            await storageService.SaveStorageFileAsync(entity, file.FileName, file.ContentType, storageFileStream, ct);
        }

        dto = await mapping.ToDtoAsync(entity, new ProfilePicture(), ct);

        if (!await useCase.UpdateAsync(entity, dto, ct))
            return new BaseResponseT<ProfilePicture>() { Error = BaseResponseErrorEnum.ErrorUpdatingState };

        return new BaseResponseT<ProfilePicture>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<bool>> FileDelete(Guid profilepictureId, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(profilepictureId, ct);

        if (entity == null)
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        var dto = await mapping.ToDtoAsync(entity, new ProfilePicture(), ct);

        if (!await useCase.CanDeleteAsync(dto, ct))
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        await storageService.DeleteStorageFileAsync(entity, ct);

        dto = await mapping.ToDtoAsync(entity, new ProfilePicture(), ct);

        if (!await useCase.UpdateAsync(entity, dto, ct))
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorUpdatingState };

        return new BaseResponseT<bool>() 
        { 
            Success = true,
            Response = true 
        };
    }
}