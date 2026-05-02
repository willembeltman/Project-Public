using UwvLlm.App.Interfaces;

namespace UwvLlm.App.Services;

public class EmailService(
    IUiService ui) : IEmailService
{
    public async Task Send(string? from, string? to, string? subject, string? body)
    {
        await ui.ShowAlert("OK", $"Mail naar {to}", "OK");
    }
}
