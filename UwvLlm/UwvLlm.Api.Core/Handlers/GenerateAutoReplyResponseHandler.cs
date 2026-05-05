using Microsoft.EntityFrameworkCore;
using UwvLlm.Shared.Private.Dtos;
using UwvLlm.Shared.Private.Interfaces;
using UwvLlm.Shared.Public.Dtos;
using UwvLlm.Shared.Public.Enums;
using UwvLlm.Shared.Public.Interfaces;

namespace UwvLlm.Llm.Core.Handlers;

public class GenerateAutoReplyResponseHandler(
    IDbContextFactory<Api.Core.Infrastructure.Data.ApplicationDbContext> dbFactory,
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
