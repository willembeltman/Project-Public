using gAPI.Interfaces;
using System.Windows.Input;
using UwvLlm.App.Interfaces;
using UwvLlm.App.Pages;

namespace UwvLlm.App.ViewModels;

public class RegisterViewModel : BaseViewModel
{
    private readonly IUiService Ui;

    public RegisterViewModel(
        IUiService ui,
        IAccountService accountService)
    {
        Ui = ui;
        RegisterCommand = new Command(async () => await Register());
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

    public string? PasswordRepeat
    {
        get => field;
        set => SetProperty(ref field, value);
    }

    public ICommand RegisterCommand { get; }

    private async Task Register()
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            await Ui.ShowAlert("Fout", "Email verplicht", "OK");
            return;
        }

        await Ui.NavigateToAsync<EmailPage>();
    }
}