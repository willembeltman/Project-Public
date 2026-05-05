using Microsoft.EntityFrameworkCore;
using UwvLlm.Api.Core.Dtos;
using UwvLlm.Infrastructure.Messaging.Interfaces;
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
        using var db = await dbFactory.CreateDbContextAsync();
        var dbMailMessage = await db.MailMessages.FirstOrDefaultAsync(a => a.Id == message.Email.Id, ct);
        if (dbMailMessage == null)
            return;

        var userNotification = new UserNotification()
        {
            ExternalType = NotificationType.Mail,
            ExternalId = dbMailMessage.Id.ToString(), 
            Title = "Message received",
            Message = $@"Subject: {dbMailMessage.Subject}

{dbMailMessage.Content}

==========================
        AUTO-REPLY
==========================

{dbMailMessage.AutoResponse}

Do you want to auto-reply?",
            QuickOptions = ["Yes", "No", "Modify"]
        };

        var createResult = await notificationService.Create(userNotification, ct);
        if (createResult.Success == false || createResult.Response == null) return;

        await notificationHub.ToAll.OnNotificationReceived(createResult.Response);
    }
}
