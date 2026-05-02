using System.Windows.Input;
using UwvLlm.App.Interfaces;

namespace UwvLlm.App.ViewModels;

public class RegisterViewModel : BaseViewModel
{
    public RegisterViewModel(
        IAuthenticationService authentication)
    {
        RegisterCommand = new Command(async () => await authentication.Register(Email, Password, PasswordRepeat));
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
}