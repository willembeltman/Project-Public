using gAPI.Core.Attributes;
using gAPI.Core.Dtos;
using Microsoft.AspNetCore.Http;
using TinderWithStats.Shared.Dtos;

namespace TinderWithStats.Shared.Interfaces;

[GenerateApi]
[IsAuthorized]
public interface IProfilePicturesApi
{
    [IsCreate]
    Task<BaseResponseT<ProfilePicture>> Create(ProfilePicture profilepicture, CancellationToken ct);

    [IsRead]
    Task<BaseResponseT<ProfilePicture>> Read(Guid profilepictureId, CancellationToken ct);

    [IsUpdate]
    Task<BaseResponseT<ProfilePicture>> Update(ProfilePicture profilepicture, CancellationToken ct);

    [IsDelete(typeof(ProfilePicture))]
    Task<BaseResponseT<bool>> Delete(Guid profilepictureId, CancellationToken ct);

    [IsList]
    Task<BaseListResponseT<ProfilePicture>> List(int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListBy(nameof(ProfilePicture.ProfileId), typeof(Profile))]
    Task<BaseListResponseT<ProfilePicture>> ListByProfileId(Guid ProfileId, int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListNotBy(nameof(ProfilePicture.ProfileId), typeof(Profile))]
    Task<BaseListResponseT<ProfilePicture>> ListNotByProfileId(Guid ProfileId, int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsFileUpdate]
    Task<BaseResponseT<ProfilePicture>> FileUpdate(Guid profilepictureId, IFormFile? file, CancellationToken ct);

    [IsFileDelete(typeof(ProfilePicture))]
    Task<BaseResponseT<bool>> FileDelete(Guid profilepictureId, CancellationToken ct);
}