using System.Windows.Input;
using UwvLlm.App.Interfaces;

namespace UwvLlm.App.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly IAuthenticationService LoginService;

    public LoginViewModel(
        IAuthenticationService loginService)
    {
        LoginService = loginService;
        LoginCommand = new Command(async () => await LoginService.Login(Email, Password));
        GotoRegisterCommand = new Command(async () => await LoginService.GotoRegister());
    }

    public ICommand LoginCommand { get; }
    public ICommand GotoRegisterCommand { get; }

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
}