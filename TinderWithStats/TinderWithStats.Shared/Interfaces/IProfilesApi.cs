using gAPI.Core.Attributes;
using gAPI.Core.Dtos;
using TinderWithStats.Shared.Dtos;

namespace TinderWithStats.Shared.Interfaces;

[GenerateApi]
[IsAuthorized]
public interface IProfilesApi
{
    [IsCreate]
    Task<BaseResponseT<Profile>> Create(Profile profile, CancellationToken ct);

    [IsRead]
    Task<BaseResponseT<Profile>> Read(Guid profileId, CancellationToken ct);

    [IsUpdate]
    Task<BaseResponseT<Profile>> Update(Profile profile, CancellationToken ct);

    [IsDelete(typeof(Profile))]
    Task<BaseResponseT<bool>> Delete(Guid profileId, CancellationToken ct);

    [IsList]
    Task<BaseListResponseT<Profile>> List(int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListBy(nameof(Profile.UserId), typeof(User))]
    Task<BaseListResponseT<Profile>> ListByUserId(Guid UserId, int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListNotBy(nameof(Profile.UserId), typeof(User))]
    Task<BaseListResponseT<Profile>> ListNotByUserId(Guid UserId, int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListBy(nameof(Profile.LocationId), typeof(Location))]
    Task<BaseListResponseT<Profile>> ListByLocationId(int LocationId, int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListNotBy(nameof(Profile.LocationId), typeof(Location))]
    Task<BaseListResponseT<Profile>> ListNotByLocationId(int LocationId, int? skip, int? take, string[]? orderby, CancellationToken ct);
}