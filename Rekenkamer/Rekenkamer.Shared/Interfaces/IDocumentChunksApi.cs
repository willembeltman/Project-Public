using gAPI.Core.Attributes;
using gAPI.Core.Dtos;
using Rekenkamer.Shared.Dtos;

namespace Rekenkamer.Shared.Interfaces;

[GenerateApi]
[IsAuthorized]
public interface IDocumentChunksApi
{
    [IsCreate]
    Task<BaseResponseT<DocumentChunk>> Create(DocumentChunk documentchunk, CancellationToken ct);

    [IsRead]
    Task<BaseResponseT<DocumentChunk>> Read(Guid documentchunkId, CancellationToken ct);

    [IsUpdate]
    Task<BaseResponseT<DocumentChunk>> Update(DocumentChunk documentchunk, CancellationToken ct);

    [IsDelete(typeof(DocumentChunk))]
    Task<BaseResponseT<bool>> Delete(Guid documentchunkId, CancellationToken ct);

    [IsList]
    Task<BaseListResponseT<DocumentChunk>> List(int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListBy(nameof(DocumentChunk.DocumentId), typeof(Document))]
    Task<BaseListResponseT<DocumentChunk>> ListByDocumentId(Guid DocumentId, int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListNotBy(nameof(DocumentChunk.DocumentId), typeof(Document))]
    Task<BaseListResponseT<DocumentChunk>> ListNotByDocumentId(Guid DocumentId, int? skip, int? take, string[]? orderby, CancellationToken ct);
}