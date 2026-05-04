using gAPI.Attributes;
using UwvLlm.Shared.Dtos;

namespace UwvLlm.Shared.Interfaces;

[GenerateApi]
[IsAuthorized]
public interface IMailApi
{
    Task Receive(NewMailMessage email, CancellationToken ct);
}
