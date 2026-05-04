using UwvLlm.Shared.Dtos;

namespace UwvLlm.Core.Interfaces;

public interface IMailService
{
    Task<MailMessage> ReceivedEmail(NewMailMessage newMail, CancellationToken ct);
}