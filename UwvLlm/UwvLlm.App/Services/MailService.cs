using UwvLlm.App.Interfaces;
using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.App.Services;

public class MailService(
    IUiService ui,
    IMailApi email,
    INavigationService navigation) 
    : IMailService
{
    public async Task Send(Guid? toUserId, string? subject, string? body)
    {
        if (toUserId == null)
        {
            await ui.ShowAlert("Fout", "to verplicht", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(subject))
        {
            await ui.ShowAlert("Fout", "subject verplicht", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(body))
        {
            await ui.ShowAlert("Fout", "body verplicht", "OK");
            return;
        }

        var mail = new MailMessage()
        {
            ToUserId = toUserId.Value,
            Subject = subject,
            Body = body
        };
        await email.SendMail(mail, CancellationToken.None);

        await ui.ShowAlert("OK", $"Mail verstuurd naar {toUserId}", "OK");
        await navigation.GotoMainPageAsync();
    }
}
