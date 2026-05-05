using Microsoft.EntityFrameworkCore;
using UwvLlm.Api.Core.Dtos;
using UwvLlm.Api.Core.Interfaces;
using UwvLlm.Shared.CrudInterfaces;
using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Enums;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.LlmProxy.Core.Handlers;

public class GenerateAutoReplyResponseHandler(
    IDbContextFactory<Infrastructure.Data.Entities.ApplicationDbContext> dbFactory,
    IUserNotificationCrudService notificationService,
    INotificationHubContext notificationHub)
    : IHandler<GenerateAutoReplyResponse>
{
    public async Task Handle(GenerateAutoReplyResponse message, CancellationToken ct)
    {
        using var db = dbFactory.CreateDbContext();

        var userNotification = new UserNotification()
        {
            ExternalType = NotificationType.Mail,
            ExternalId = message.ExternalId, //mailResponse.Response.Id.ToString(),
            Title = "Message received",
            Message = "Message content\r\nDo you want to auto-reply?",
            QuickOptions = ["Yes", "No", "Modify"]
        };
        var notification = await notificationService.Create(userNotification, ct);
        if (notification.Success == false || notification.Response == null) return;
        await notificationHub.ToAll.OnNotificationReceived(notification.Response);
    }
}
