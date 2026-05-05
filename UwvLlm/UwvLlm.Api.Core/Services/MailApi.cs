using gAPI.Core.Server;
using UwvLlm.Shared.Private.Dtos;
using UwvLlm.Shared.Public.Dtos;
using UwvLlm.Shared.Public.Interfaces;

namespace UwvLlm.Api.Core.Services;

public class MailApi(
    IAuthenticationService<Infrastructure.Data.User, State> authenticationService,
    IMailMessageCrudService mailService,
    ServiceBusSenderService serviceBusSender) //,
    //IUserNotificationCrudService notificationService,
    //INotificationHubContext notificationHub)
    : IMailApi
{
    public async Task SendMail(MailMessage newMail, CancellationToken ct)
    {
        if (authenticationService.State.User == null)
            return;

        newMail.FromUserId = authenticationService.State.User.Id;
        var mailResponse = await mailService.Create(newMail, ct);
        if (mailResponse.Success == false || mailResponse.Response == null) 
            return;

        var request = new GenerateAutoReplyRequest();
        await serviceBusSender.SendGenerateAutoReplyRequest(request, ct);

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
