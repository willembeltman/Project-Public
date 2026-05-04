using UwvLlm.App.Interfaces;
using UwvLlm.App.Pages;
using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.App.Services;

public class MailService(
    IUiService ui,
    IMailApi email,
    INavigationService navigation) 
    : IMailService
{
    public async Task Send(string? from, string? to, string? subject, string? body)
    {
        if (string.IsNullOrWhiteSpace(from))
        {
            await ui.ShowAlert("Fout", "from verplicht", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(to))
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

        //await email.Receive(new EmailDto(from, [to], subject, body));
        await ui.ShowAlert("OK", $"Mail verstuurd naar {to}", "OK");
        await navigation.GotoMainPageAsync();
    }
}
