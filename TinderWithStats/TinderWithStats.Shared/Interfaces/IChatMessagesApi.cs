using gAPI.Core.Attributes;
using gAPI.Core.Dtos;
using TinderWithStats.Shared.Dtos;

namespace TinderWithStats.Shared.Interfaces;

[GenerateApi]
public interface IChatMessagesApi
{
    [IsCreate]
    Task<BaseResponseT<ChatMessage>> Create(ChatMessage chatmessage, CancellationToken ct);

    [IsRead]
    Task<BaseResponseT<ChatMessage>> Read(Guid chatmessageId, CancellationToken ct);

    [IsUpdate]
    Task<BaseResponseT<ChatMessage>> Update(ChatMessage chatmessage, CancellationToken ct);

    [IsDelete(typeof(ChatMessage))]
    Task<BaseResponseT<bool>> Delete(Guid chatmessageId, CancellationToken ct);

    [IsList]
    Task<BaseListResponseT<ChatMessage>> List(int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListBy(nameof(ChatMessage.MatchId), typeof(Match))]
    Task<BaseListResponseT<ChatMessage>> ListByMatchId(Guid MatchId, int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListNotBy(nameof(ChatMessage.MatchId), typeof(Match))]
    Task<BaseListResponseT<ChatMessage>> ListNotByMatchId(Guid MatchId, int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListBy(nameof(ChatMessage.ProfileSenderId), typeof(Profile))]
    Task<BaseListResponseT<ChatMessage>> ListByProfileSenderId(Guid ProfileSenderId, int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListNotBy(nameof(ChatMessage.ProfileSenderId), typeof(Profile))]
    Task<BaseListResponseT<ChatMessage>> ListNotByProfileSenderId(Guid ProfileSenderId, int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListBy(nameof(ChatMessage.ProfileReceiverId), typeof(Profile))]
    Task<BaseListResponseT<ChatMessage>> ListByProfileReceiverId(Guid ProfileReceiverId, int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListNotBy(nameof(ChatMessage.ProfileReceiverId), typeof(Profile))]
    Task<BaseListResponseT<ChatMessage>> ListNotByProfileReceiverId(Guid ProfileReceiverId, int? skip, int? take, string[]? orderby, CancellationToken ct);
}