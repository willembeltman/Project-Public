using gAPI.Attributes;
using gAPI.Dtos;
using UwvLlm.Shared.Dtos;

namespace UwvLlm.Shared.Interfaces;

[GenerateApi]
public interface IMailMessagesService
{
    [IsCreate]
    Task<BaseResponseT<MailMessage>> Create(MailMessage emailmessage, CancellationToken ct);

    [IsRead]
    Task<BaseResponseT<MailMessage>> Read(Guid emailmessageId, CancellationToken ct);

    [IsUpdate]
    Task<BaseResponseT<MailMessage>> Update(MailMessage emailmessage, CancellationToken ct);

    [IsDelete(typeof(MailMessage))]
    Task<BaseResponseT<bool>> Delete(Guid emailmessageId, CancellationToken ct);

    [IsList]
    Task<BaseListResponseT<MailMessage>> List(int? skip, int? take, string[]? orderby, CancellationToken ct);
}