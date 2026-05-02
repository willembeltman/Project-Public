using System.Windows.Input;
using UwvLlm.App.Interfaces;

namespace UwvLlm.App.ViewModels;

public class RegisterViewModel : BaseViewModel
{
    public RegisterViewModel(
        INavigationService navigation,
        IAuthenticationService authentication)
    {
        RegisterCommand = new Command(async () => await authentication.RegisterAsync(UserName, Email, Password, PasswordRepeat));
        GotoLoginCommand = new Command(async () => await navigation.GotoLoginPageAsync());
    }

    public string? UserName
    {
        get => field;
        set => SetProperty(ref field, value);
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
    public ICommand GotoLoginCommand { get; }
}