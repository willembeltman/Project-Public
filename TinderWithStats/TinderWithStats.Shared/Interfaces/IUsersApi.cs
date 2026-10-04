using gAPI.Core.Attributes;
using gAPI.Core.Dtos;
using TinderWithStats.Shared.Dtos;

namespace TinderWithStats.Shared.Interfaces;

[GenerateApi]
[IsAuthorized]
public interface IUsersApi
{
    [IsCreate]
    Task<BaseResponseT<User>> Create(User user, CancellationToken ct);

    [IsRead]
    Task<BaseResponseT<User>> Read(Guid userId, CancellationToken ct);

    [IsUpdate]
    Task<BaseResponseT<User>> Update(User user, CancellationToken ct);

    [IsDelete(typeof(User))]
    Task<BaseResponseT<bool>> Delete(Guid userId, CancellationToken ct);

    [IsList]
    Task<BaseListResponseT<User>> List(int? skip, int? take, string[]? orderby, CancellationToken ct);
}