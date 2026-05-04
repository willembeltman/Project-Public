using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.Core.Services;

public class EmailApi (
    INotificationHubContext notificationHub)
    : IMailApi
{
    public Task Receive(MailMessage email)
    {
        return notificationHub.ToAll.OnNotificationReceived(new Shared.Dtos.Notification("Message received", "Message content"));
        //throw new NotImplementedException();
    }
}
