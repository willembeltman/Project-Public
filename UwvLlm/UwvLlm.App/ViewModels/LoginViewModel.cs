using gAPI.Interfaces;
using System.Windows.Input;
using UwvLlm.App.Interfaces;
using UwvLlm.App.Pages;

namespace UwvLlm.App.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly IUiService Ui;
    private readonly IAccountService AccountService;

    public LoginViewModel(
        IUiService ui,
        IAccountService accountService)
    {
        Ui = ui;
        AccountService = accountService;
        LoginCommand = new Command(async () => await Login());
        GotoRegisterCommand = new Command(async () => await GotoRegister());
    }

    public string? Email
    {
        get => field;
        set => SetProperty(ref field, value);
    }

    public string? Password
    {
        get => field;
        set => SetProperty(ref field, value);
    }

    public ICommand LoginCommand { get; }
    public ICommand GotoRegisterCommand { get; }


    private async Task Login()
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            await Ui.ShowAlert("Fout", "Email verplicht", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(Password))
        {
            await Ui.ShowAlert("Fout", "Password verplicht", "OK");
            return;
        }

        var response = await AccountService.LoginAsync(Email, Password, CancellationToken.None);
        if (response.Success == false)
        {
            await Ui.ShowAlert("Fout", $"Fout opgetreden: {response.Error}", "OK");
            return;
        }

        await Ui.NavigateToAsync<MainPage>();
    }
    private async Task GotoRegister()
    {
        await Ui.NavigateToAsync<RegisterPage>();
    }
}