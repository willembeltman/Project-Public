using gAPI.Attributes;
using gAPI.Dtos;
using UwvLlm.Shared.Dtos;

namespace UwvLlm.Shared.Interfaces;

public interface IMailMessageToUsersService
{
    [IsCreate]
    Task<BaseResponseT<MailMessageToUser>> Create(MailMessageToUser mailmessagetouser, CancellationToken ct);

    [IsRead]
    Task<BaseResponseT<MailMessageToUser>> Read(long mailmessagetouserId, CancellationToken ct);

    [IsUpdate]
    Task<BaseResponseT<MailMessageToUser>> Update(MailMessageToUser mailmessagetouser, CancellationToken ct);

    [IsDelete(typeof(MailMessageToUser))]
    Task<BaseResponseT<bool>> Delete(long mailmessagetouserId, CancellationToken ct);

    [IsList]
    Task<BaseListResponseT<MailMessageToUser>> List(int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListBy(nameof(MailMessageToUser.MailMessageId), typeof(MailMessage))]
    Task<BaseListResponseT<MailMessageToUser>> ListByMailMessageId(Guid MailMessageId, int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListNotBy(nameof(MailMessageToUser.MailMessageId), typeof(MailMessage))]
    Task<BaseListResponseT<MailMessageToUser>> ListNotByMailMessageId(Guid MailMessageId, int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListBy(nameof(MailMessageToUser.UserId), typeof(User))]
    Task<BaseListResponseT<MailMessageToUser>> ListByUserId(Guid UserId, int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListNotBy(nameof(MailMessageToUser.UserId), typeof(User))]
    Task<BaseListResponseT<MailMessageToUser>> ListNotByUserId(Guid UserId, int? skip, int? take, string[]? orderby, CancellationToken ct);
}