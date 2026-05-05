using UwvLlm.Shared.Public.Dtos;
using UwvLlm.Shared.Public.Interfaces;

namespace UwvLlm.Api.Core.Services;

public class MailApi(
    IMailMessageCrudService mailService,
    ServiceBusSenderService serviceBusSender) //,
    //IUserNotificationCrudService notificationService,
    //INotificationHubContext notificationHub)
    : IMailApi
{
    public async Task SendMail(MailMessage newMail, CancellationToken ct)
    {
        var mailResponse = await mailService.Create(newMail, ct);
        if (mailResponse.Success == false || mailResponse.Response == null) return;

        await serviceBusSender.SendAsync(ct);

        //var userNotification = new UserNotification()
        //{
        //    ExternalType = NotificationType.Mail,
        //    ExternalId = mailResponse.Response.Id.ToString(),
        //    Title = "Message received",
        //    Message = "Message content\r\nDo you want to auto-reply?",
        //    QuickOptions = ["Yes", "No", "Modify"]
        //};
        //var notification = await notificationService.Create(userNotification, ct);
        //if (notification.Success == false || notification.Response == null) return;
        //await notificationHub.ToAll.OnNotificationReceived(notification.Response);
    }
}
