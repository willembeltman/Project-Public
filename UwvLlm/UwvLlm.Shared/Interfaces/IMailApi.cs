using gAPI.Core.Attributes;
using UwvLlm.Shared.Dtos;

namespace UwvLlm.Shared.Interfaces;

[GenerateApi]
[IsAuthorized]
public interface IMailApi
{
    Task SendMail(MailMessage email, CancellationToken ct);
}
