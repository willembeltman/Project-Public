using gAPI.Core.Attributes;
using gAPI.Core.Dtos;
using TinderWithStats.Shared.Dtos;

namespace TinderWithStats.Shared.Interfaces;

[GenerateApi]
[IsAuthorized]
public interface ILocationsApi
{
    [IsCreate]
    Task<BaseResponseT<Location>> Create(Location location, CancellationToken ct);

    [IsRead]
    Task<BaseResponseT<Location>> Read(int locationId, CancellationToken ct);

    [IsUpdate]
    Task<BaseResponseT<Location>> Update(Location location, CancellationToken ct);

    [IsDelete(typeof(Location))]
    Task<BaseResponseT<bool>> Delete(int locationId, CancellationToken ct);

    [IsList]
    Task<BaseListResponseT<Location>> List(int? skip, int? take, string[]? orderby, CancellationToken ct);
}