using gAPI.Core.Attributes;
using gAPI.Core.Dtos;
using TinderWithStats.Shared.Dtos;

namespace TinderWithStats.Shared.Interfaces;

[GenerateApi]
[IsAuthorized]
public interface IMatchsApi
{
    [IsCreate]
    Task<BaseResponseT<Match>> Create(Match match, CancellationToken ct);

    [IsRead]
    Task<BaseResponseT<Match>> Read(Guid matchId, CancellationToken ct);

    [IsUpdate]
    Task<BaseResponseT<Match>> Update(Match match, CancellationToken ct);

    [IsDelete(typeof(Match))]
    Task<BaseResponseT<bool>> Delete(Guid matchId, CancellationToken ct);

    [IsList]
    Task<BaseListResponseT<Match>> List(int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListBy(nameof(Match.ProfileSenderId), typeof(Profile))]
    Task<BaseListResponseT<Match>> ListByProfileSenderId(Guid ProfileSenderId, int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListNotBy(nameof(Match.ProfileSenderId), typeof(Profile))]
    Task<BaseListResponseT<Match>> ListNotByProfileSenderId(Guid ProfileSenderId, int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListBy(nameof(Match.ProfileReceiverId), typeof(Profile))]
    Task<BaseListResponseT<Match>> ListByProfileReceiverId(Guid ProfileReceiverId, int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListNotBy(nameof(Match.ProfileReceiverId), typeof(Profile))]
    Task<BaseListResponseT<Match>> ListNotByProfileReceiverId(Guid ProfileReceiverId, int? skip, int? take, string[]? orderby, CancellationToken ct);
}