using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Enums;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.Core.Services;

public class MailApi (
    IMailMessagesService mailService,
    IUserNotificationsService notificationService,
    INotificationHubContext notificationHub)
    : IMailApi
{
    public async Task SendMail(MailMessage newMail, CancellationToken ct)
    {
        var mailResponse = await mailService.Create(newMail, ct);
        if (mailResponse.Success == false || mailResponse.Response == null) return;
        var mail = mailResponse.Response;

        var userNotification = new UserNotification()
        {
            ExternalType = NotificationType.Mail,
            ExternalId= mail.Id.ToString(),
            Title ="Message received",
            Message="Message content\r\nDo you want to auto-reply?",
            //["Yes", "No", "Modify"]
            
        };
        var notification = await notificationService.Create(userNotification, ct);
        if (notification.Success == false || notification.Response == null) return;



        await notificationHub.ToAll.OnNotificationReceived(notification.Response);
    }
}
