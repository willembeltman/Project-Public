using gAPI.Core.Client;
using gAPI.Core.Interfaces;
using UwvLlm.App.Core.Interfaces;
using UwvLlm.Shared.Dtos;

namespace UwvLlm.App.Core.Services;

public class AuthenticationService(
    IUiService uiService,
    IAccountService accountService,
    IAuthenticatedHttpClient<State> httpClient,
    INavigationService navigationService) 
    : IAuthenticationService
{
    public Task<bool> IsAuthenticatedAsync()
    {
        return httpClient.IsAuthenticatedAsync(CancellationToken.None);
    }

    public async Task RegisterAsync(string? username, string? email, string? password, string? passwordRepeat)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            await uiService.ShowAlert("Fout", "username verplicht", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(email))
        {
            await uiService.ShowAlert("Fout", "Email verplicht", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            await uiService.ShowAlert("Fout", "password verplicht", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(passwordRepeat))
        {
            await uiService.ShowAlert("Fout", "passwordRepeat verplicht", "OK");
            return;
        }
        if (password != passwordRepeat)
        {
            await uiService.ShowAlert("Fout", "password en passwordRepeat zijn niet gelijk", "OK");
            return;
        }
        var response = await accountService.RegisterAsync(username, email, password, passwordRepeat, CancellationToken.None);
        if (response.Success == false)
        {
            await uiService.ShowAlert("Fout", $"Fout opgetreden: {response.Error}", "OK");
            return;
        }
        await navigationService.GotoMainPageAsync();
    }

    public async Task LoginAsync(string? email, string? password)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            await uiService.ShowAlert("Fout", "Email verplicht", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            await uiService.ShowAlert("Fout", "Password verplicht", "OK");
            return;
        }
        var response = await accountService.LoginAsync(email, password, CancellationToken.None);
        if (response.Success == false)
        {
            await uiService.ShowAlert("Fout", $"Fout opgetreden: {response.Error}", "OK");
            return;
        }
        await navigationService.GotoMainPageAsync();
    }
}
