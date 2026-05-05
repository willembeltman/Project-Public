using gAPI.Core.Attributes;
using gAPI.Core.Dtos;
using Microsoft.AspNetCore.Http;
using UwvLlm.Shared.Public.Dtos;

namespace UwvLlm.Shared.Public.Interfaces;

[GenerateApi]
[IsAuthorized]
public interface IUserCrudService
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

    [IsFileUpdate]
    Task<BaseResponseT<User>> FileUpdate(Guid userId, IFormFile? file, CancellationToken ct);

    [IsFileDelete(typeof(User))]
    Task<BaseResponseT<bool>> FileDelete(Guid userId, CancellationToken ct);
}