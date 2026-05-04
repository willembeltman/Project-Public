using gAPI.Core.Client;
using gAPI.Interfaces;
using UwvLlm.App.Core.Interfaces;
using UwvLlm.Shared.Dtos;

namespace UwvLlm.App.Core.Services;

public class AuthenticationService(
    IUiService ui,
    IAccountService accountService,
    IAuthenticatedHttpClient<State> httpClient,
    INavigationService navigation)
{
    public Task<bool> IsAuthenticatedAsync()
    {
        return httpClient.IsAuthenticatedAsync(CancellationToken.None);
    }

    public async Task RegisterAsync(string? username, string? email, string? password, string? passwordRepeat)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            await ui.ShowAlert("Fout", "username verplicht", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(email))
        {
            await ui.ShowAlert("Fout", "Email verplicht", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            await ui.ShowAlert("Fout", "password verplicht", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(passwordRepeat))
        {
            await ui.ShowAlert("Fout", "passwordRepeat verplicht", "OK");
            return;
        }
        if (password != passwordRepeat)
        {
            await ui.ShowAlert("Fout", "password en passwordRepeat zijn niet gelijk", "OK");
            return;
        }
        var response = await accountService.RegisterAsync(username, email, password, passwordRepeat, CancellationToken.None);
        if (response.Success == false)
        {
            await ui.ShowAlert("Fout", $"Fout opgetreden: {response.Error}", "OK");
            return;
        }
        await navigation.GotoMainPageAsync();
    }

    public async Task LoginAsync(string? email, string? password)
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
        await navigation.GotoMainPageAsync();
    }
}
