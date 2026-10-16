using gAPI.Core.Dtos;
using gAPI.Core.Enums;
using Rekenkamer.Shared.Dtos;
using Rekenkamer.Shared.Interfaces;

namespace Rekenkamer.Infrastructure.Data.Services;

public class DocumentsApi(
    gAPI.Core.Interfaces.IUseCase<Rekenkamer.Infrastructure.Data.Entities.Document, Document, Guid> useCase,
    gAPI.Core.Interfaces.Mapping<Rekenkamer.Infrastructure.Data.Entities.Document, Document> mapping)
    : IDocumentsApi
{
    public async Task<BaseResponseT<Document>> Create(Document dto, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<Document>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByMatchAsync(dto, ct);

        if (entity != null)
            return new BaseResponseT<Document>() { Error = BaseResponseErrorEnum.ErrorAlreadyUsed };

        entity = mapping.ToEntity(dto, new Rekenkamer.Infrastructure.Data.Entities.Document());

        if (!await useCase.CanCreateAsync(dto, ct))
            return new BaseResponseT<Document>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.AddAsync(entity, ct))
            return new BaseResponseT<Document>() { Error = BaseResponseErrorEnum.ErrorAttachingState };

        dto = await mapping.ToDtoAsync(entity, new Document(), ct);

        return new BaseResponseT<Document>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<Document>> Read(Guid documentId, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<Document>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(documentId, ct);
        if (entity == null)
            return new BaseResponseT<Document>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        var dto = await mapping.ToDtoAsync(entity, new Document(), ct);

        if (!await useCase.CanReadAsync(dto, ct))
            return new BaseResponseT<Document>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        return new BaseResponseT<Document>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<Document>> Update(Document dto, CancellationToken ct)
    {
        if (dto == null)
            return new BaseResponseT<Document>() { Error = BaseResponseErrorEnum.ErrorItemNotSupplied };

        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<Document>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(dto.Id, ct);
        if (entity == null)
            return new BaseResponseT<Document>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        if (!await useCase.CanUpdateAsync(dto, ct))
            return new BaseResponseT<Document>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        mapping.ToEntity(dto, entity);

        if (!await useCase.UpdateAsync(entity, dto, ct))
            return new BaseResponseT<Document>() { Error = BaseResponseErrorEnum.ErrorUpdatingState };

        dto = await mapping.ToDtoAsync(entity, dto, ct);

        return new BaseResponseT<Document>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<bool>> Delete(Guid documentId, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(documentId, ct);
        if (entity == null)
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        var dto = await mapping.ToDtoAsync(entity, new Document(), ct);

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

    public async Task<BaseListResponseT<Document>> List(int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<Document>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<Document>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase.ListAll();

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<Document>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<Document>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = await dtos.ToArrayAsync(ct)
        };
    }
}