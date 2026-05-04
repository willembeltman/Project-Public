using UwvLlm.Shared.Dtos;

namespace UwvLlm.Core.Interfaces;

public interface IMailService
{
    MailMessage ReceivedEmail(NewMailMessage newMail);
}