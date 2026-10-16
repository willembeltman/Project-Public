using gAPI.Core.Attributes;
using gAPI.Core.Dtos;
using Rekenkamer.Shared.Dtos;

namespace Rekenkamer.Shared.Interfaces;

[GenerateApi]
[IsAuthorized]
public interface IDocumentsApi
{
    [IsCreate]
    Task<BaseResponseT<Document>> Create(Document document, CancellationToken ct);

    [IsRead]
    Task<BaseResponseT<Document>> Read(Guid documentId, CancellationToken ct);

    [IsUpdate]
    Task<BaseResponseT<Document>> Update(Document document, CancellationToken ct);

    [IsDelete(typeof(Document))]
    Task<BaseResponseT<bool>> Delete(Guid documentId, CancellationToken ct);

    [IsList]
    Task<BaseListResponseT<Document>> List(int? skip, int? take, string[]? orderby, CancellationToken ct);
}