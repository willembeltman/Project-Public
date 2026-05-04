using System.Windows.Input;
using UwvLlm.App.Core.Interfaces;
using UwvLlm.App.Core.Services;

namespace UwvLlm.App.Core.ViewModels;

public class RegisterViewModel : BaseViewModel
{
    public RegisterViewModel(
        INavigationService navigationService,
        AuthenticationService authenticationService)
    {
        RegisterCommand = new RelayCommand(async () => await authenticationService.RegisterAsync(UserName, Email, Password, PasswordRepeat));
        GotoLoginCommand = new RelayCommand(async () => await navigationService.GotoLoginPageAsync());
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