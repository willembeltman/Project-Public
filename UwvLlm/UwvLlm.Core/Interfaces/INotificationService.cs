using UwvLlm.Shared.Dtos;

namespace UwvLlm.Core.Interfaces;

public interface INotificationService
{
    Notification CreateEmailNotification(Task<MailMessage> mail, CancellationToken ct);
}