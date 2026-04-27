using gAPI.Core.Client;
using gAPI.Dtos;
using System.Windows.Input;
using UwvLlm.App.Interfaces;
using UwvLlm.App.Pages;

namespace UwvLlm.App.ViewModels;

public class MainPageViewModel
{
    private readonly IUiService Ui;
    private readonly IAuthenticatedHttpClient<AuthStateUserDto> AuthenticationService;

    public MainPageViewModel(
        IUiService ui,
        IAuthenticatedHttpClient<AuthStateUserDto> auth)
    {
        Ui = ui;
        AuthenticationService = auth;
        GotoSendEmailPageCommand = new Command(async () => await GotoSendEmailPage());
    }

    public ICommand GotoSendEmailPageCommand { get; }

    private async Task GotoSendEmailPage()
    {
        await Ui.NavigateToAsync<EmailPage>();
    }
}
