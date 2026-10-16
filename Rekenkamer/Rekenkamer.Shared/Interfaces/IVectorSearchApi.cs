using gAPI.Core.Attributes;
using gAPI.Core.Dtos;
using Rekenkamer.Shared.Dtos;

namespace Rekenkamer.Shared.Interfaces;

[GenerateApi]
public interface IVectorSearchApi
{
    Task<BaseListResponseT<VectorSearchResult>> HybridSearchAsync(string query, int skip, int take, CancellationToken ct);
    Task<BaseListResponseT<VectorSearchResult>> VectorSearchAsync(string query, int skip, int take, CancellationToken ct);
}
