using CommunityToolkit.Mvvm.Input;
using UwvLlm.App.Core.Interfaces;
using UwvLlm.App.Core.Services;

namespace UwvLlm.App.Core.ViewModels;

public partial class LoginViewModel(
    INavigationService navigationService,
    AuthenticationService authenticationService)
    : BaseViewModel
{
    public string? Email { get => field; set => SetProperty(ref field, value); }
    public string? Password { get => field; set => SetProperty(ref field, value); }

    public async Task OnAppearingAsync()
    {
        if (await authenticationService.IsAuthenticatedAsync())
            await navigationService.GotoMainPageAsync();
    }

    [RelayCommand]
    public async Task Login()
        => await authenticationService.LoginAsync(Email, Password);

    [RelayCommand]
    public async Task GotoRegister()
        => await navigationService.GotoRegisterPageAsync();
}