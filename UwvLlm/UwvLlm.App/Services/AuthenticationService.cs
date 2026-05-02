using gAPI.Interfaces;
using UwvLlm.App.Interfaces;
using UwvLlm.App.Pages;

namespace UwvLlm.App.Services;

public class AuthenticationService(
    IUiService ui,
    IAccountService accountService,
    INavigationService navigation)
    : IAuthenticationService
{
    public async Task Register(string? email, string? password, string? passwordRepeat)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            await ui.ShowAlert("Fout", "Email verplicht", "OK");
            return;
        }
        await navigation.NavigateToAsync<EmailPage>();
    }

    public async Task Login(string? email, string? password)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            await ui.ShowAlert("Fout", "Email verplicht", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            await ui.ShowAlert("Fout", "Password verplicht", "OK");
            return;
        }
        var response = await accountService.LoginAsync(email, password, CancellationToken.None);
        if (response.Success == false)
        {
            await ui.ShowAlert("Fout", $"Fout opgetreden: {response.Error}", "OK");
            return;
        }
        await navigation.NavigateToAsync<MainPage>();
    }

    public async Task GotoRegister()
    {
        await navigation.NavigateToAsync<RegisterPage>();
    }
}
