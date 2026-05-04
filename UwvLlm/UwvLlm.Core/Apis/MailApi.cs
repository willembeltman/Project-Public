using UwvLlm.Core.Interfaces;
using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.Core.Apis;

public class MailApi (
    IMailService mailService,
    INotificationService notificationService,
    INotificationHubContext notificationHub)
    : IMailApi
{
    public Task Receive(NewMailMessage newMail, CancellationToken ct)
    {
        var mail = mailService.ReceivedEmail(newMail, ct);
        var notification = notificationService.CreateEmailNotification(mail, ct);
        return notificationHub.ToAll.OnNotificationReceived(notification);
    }
}
