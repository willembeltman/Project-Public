using gAPI.Core.Dtos;
using gAPI.Core.Enums;
using Rekenkamer.Shared.Dtos;
using Rekenkamer.Shared.Interfaces;

namespace Rekenkamer.Infrastructure.Data.Services;

public class DocumentChunksApi(
    gAPI.Core.Interfaces.IUseCase<Rekenkamer.Infrastructure.Data.Entities.DocumentChunk, DocumentChunk, Guid> useCase,
    gAPI.Core.Interfaces.Mapping<Rekenkamer.Infrastructure.Data.Entities.DocumentChunk, DocumentChunk> mapping)
    : IDocumentChunksApi
{
    public async Task<BaseResponseT<DocumentChunk>> Create(DocumentChunk dto, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<DocumentChunk>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByMatchAsync(dto, ct);

        if (entity != null)
            return new BaseResponseT<DocumentChunk>() { Error = BaseResponseErrorEnum.ErrorAlreadyUsed };

        entity = mapping.ToEntity(dto, new Rekenkamer.Infrastructure.Data.Entities.DocumentChunk());

        if (!await useCase.CanCreateAsync(dto, ct))
            return new BaseResponseT<DocumentChunk>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.AddAsync(entity, ct))
            return new BaseResponseT<DocumentChunk>() { Error = BaseResponseErrorEnum.ErrorAttachingState };

        dto = await mapping.ToDtoAsync(entity, new DocumentChunk(), ct);

        return new BaseResponseT<DocumentChunk>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<DocumentChunk>> Read(Guid documentchunkId, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<DocumentChunk>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(documentchunkId, ct);
        if (entity == null)
            return new BaseResponseT<DocumentChunk>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        var dto = await mapping.ToDtoAsync(entity, new DocumentChunk(), ct);

        if (!await useCase.CanReadAsync(dto, ct))
            return new BaseResponseT<DocumentChunk>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        return new BaseResponseT<DocumentChunk>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<DocumentChunk>> Update(DocumentChunk dto, CancellationToken ct)
    {
        if (dto == null)
            return new BaseResponseT<DocumentChunk>() { Error = BaseResponseErrorEnum.ErrorItemNotSupplied };

        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<DocumentChunk>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(dto.Id, ct);
        if (entity == null)
            return new BaseResponseT<DocumentChunk>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        if (!await useCase.CanUpdateAsync(dto, ct))
            return new BaseResponseT<DocumentChunk>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        mapping.ToEntity(dto, entity);

        if (!await useCase.UpdateAsync(entity, dto, ct))
            return new BaseResponseT<DocumentChunk>() { Error = BaseResponseErrorEnum.ErrorUpdatingState };

        dto = await mapping.ToDtoAsync(entity, dto, ct);

        return new BaseResponseT<DocumentChunk>() 
        { 
            Success = true,
            Response = dto
        };
    }

    public async Task<BaseResponseT<bool>> Delete(Guid documentchunkId, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entity = await useCase.FindByIdAsync(documentchunkId, ct);
        if (entity == null)
            return new BaseResponseT<bool>() { Error = BaseResponseErrorEnum.ErrorItemNotFound };

        var dto = await mapping.ToDtoAsync(entity, new DocumentChunk(), ct);

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

    public async Task<BaseListResponseT<DocumentChunk>> List(int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<DocumentChunk>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<DocumentChunk>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase.ListAll();

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<DocumentChunk>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<DocumentChunk>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = await dtos.ToArrayAsync(ct)
        };
    }

    public async Task<BaseListResponseT<DocumentChunk>> ListByDocumentId(Guid documentId, int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<DocumentChunk>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<DocumentChunk>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase
            .ListAll()
            .Where(a => a.DocumentId == documentId);

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<DocumentChunk>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<DocumentChunk>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = await dtos.ToArrayAsync(ct)
        };
    }

    public async Task<BaseListResponseT<DocumentChunk>> ListNotByDocumentId(Guid documentId, int? skip, int? take, string[]? orderby, CancellationToken ct)
    {
        if (!await useCase.IsAllowedAsync(ct))
            return new BaseListResponseT<DocumentChunk>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        if (!await useCase.CanListAsync(ct))
            return new BaseListResponseT<DocumentChunk>() { Error = BaseResponseErrorEnum.ErrorNotAuthorized };

        var entities = useCase
            .ListAll()
            .Where(a => a.DocumentId != documentId);

        orderby = orderby == null || orderby.Length == 0 ? ["Id"] : orderby;
        var dtos = mapping.ProjectToDtosAsync(entities, orderby, skip, take, ct);

        if (dtos == null)
            return new BaseListResponseT<DocumentChunk>() { Error = BaseResponseErrorEnum.ErrorGettingData };

        return new BaseListResponseT<DocumentChunk>()
        {
            Success = true,
            Skip = skip ?? 0,
            Take = take ?? 0,
            CanCreate = await useCase.CanCreateAsync(ct),
            Response = await dtos.ToArrayAsync(ct)
        };
    }
}