using UwvLlm.Core.Interfaces;
using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Enums;

namespace UwvLlm.Core.Services;

public class NotificationService : INotificationService
{
    public Notification CreateEmailNotification(MailMessage mail)
    {
        return new Notification(
            NotificationType.Email,
            "1",
            "Message received",
            "Message content\r\nDo you want to auto-reply?",
            ["Yes", "No", "Modify"]);
    }
}
