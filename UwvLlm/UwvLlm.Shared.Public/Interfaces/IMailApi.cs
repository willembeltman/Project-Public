using gAPI.Core.Attributes;
using UwvLlm.Shared.Public.Dtos;

namespace UwvLlm.Shared.Public.Interfaces;

[GenerateApi]
[IsAuthorized]
public interface IMailApi
{
    Task SendMail(MailMessage email, CancellationToken ct);
}
