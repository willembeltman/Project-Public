using UwvLlm.Core.Interfaces;
using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Enums;

namespace UwvLlm.Core.Services;

public class NotificationService : INotificationService
{
    public Notification CreateEmailNotification(Task<MailMessage> mail, CancellationToken ct)
    {
        return new Notification(
            NotificationType.Mail,
            mail.Id.ToString(),
            "Message received",
            "Message content\r\nDo you want to auto-reply?",
            ["Yes", "No", "Modify"]);
    }
}
