using UwvLlm.Shared.Dtos;

namespace UwvLlm.Core.Interfaces;

public interface INotificationService
{
    Notification CreateEmailNotification(MailMessage mail);
}