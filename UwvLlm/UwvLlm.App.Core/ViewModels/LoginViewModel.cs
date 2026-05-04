using System.Windows.Input;
using UwvLlm.App.Core.Interfaces;
using UwvLlm.App.Core.Services;

namespace UwvLlm.App.Core.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly INavigationService NavigationService;
    private readonly AuthenticationService AuthenticationService;

    public LoginViewModel(
        INavigationService navigation,
        AuthenticationService authentication)
    {
        NavigationService = navigation;
        AuthenticationService = authentication;
        LoginCommand = new RelayCommand(async () => await AuthenticationService.LoginAsync(Email, Password));
        GotoRegisterCommand = new RelayCommand(async () => await NavigationService.GotoRegisterPageAsync());
    }

    public async Task OnAppearingAsync()
    {
        if (await AuthenticationService.IsAuthenticatedAsync())
            await NavigationService.GotoMainPageAsync();
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