using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.Core.Services;

public class EmailApi (
    INotificationHubContext notificationHub)
    : IMailApi
{
    public Task Receive(MailDto email)
    {
        return notificationHub.ToAll.OnNotificationReceived(new Shared.Dtos.NotificationDto("Message received", "Message content"));
        //throw new NotImplementedException();
    }
}
