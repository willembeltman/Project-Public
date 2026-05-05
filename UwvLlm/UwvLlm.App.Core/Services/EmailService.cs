using UwvLlm.App.Core.Interfaces;
using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.App.Core.Services;

public class EmailService(
    IUiService uiService,
    IMailApi mailApi,
    INavigationService navigationService) 
    : IEmailService
{
    public async Task Send(Guid? toUserId, string? subject, string? body)
    {
        if (toUserId == null)
        {
            await uiService.ShowAlert("Fout", "to verplicht", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(subject))
        {
            await uiService.ShowAlert("Fout", "subject verplicht", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(body))
        {
            await uiService.ShowAlert("Fout", "body verplicht", "OK");
            return;
        }

        var mail = new MailMessage()
        {
            ToUserId = toUserId.Value,
            Subject = subject,
            Body = body
        };
        await mailApi.SendMail(mail, CancellationToken.None);

        await uiService.ShowAlert("OK", $"Mail verstuurd naar {toUserId}", "OK");
        await navigationService.GotoMainPageAsync();
    }
}
