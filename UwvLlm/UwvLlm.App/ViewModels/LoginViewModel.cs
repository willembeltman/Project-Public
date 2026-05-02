using System.Windows.Input;
using UwvLlm.App.Interfaces;

namespace UwvLlm.App.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly INavigationService Navigation;
    private readonly IAuthenticationService Authentication;

    public LoginViewModel(
        INavigationService navigation,
        IAuthenticationService authentication)
    {
        Navigation = navigation;
        Authentication = authentication;
        LoginCommand = new Command(async () => await Authentication.LoginAsync(Email, Password));
        GotoRegisterCommand = new Command(async () => await Navigation.GotoRegisterPageAsync());
    }

    public async Task OnAppearingAsync()
    {
        if (await Authentication.IsAuthenticatedAsync())
            await Navigation.GotoMainPageAsync();
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
}