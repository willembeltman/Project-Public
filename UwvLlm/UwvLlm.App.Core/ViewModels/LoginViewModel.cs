using CommunityToolkit.Mvvm.Input;
using UwvLlm.App.Core.Interfaces;
using UwvLlm.App.Core.Services;

namespace UwvLlm.App.Core.ViewModels;

public partial class LoginViewModel(
    INavigationService navigation,
    AuthenticationService authentication)
    : BaseViewModel
{
    public string? Email { get => field; set => SetProperty(ref field, value); }
    public string? Password { get => field; set => SetProperty(ref field, value); }

    public async Task OnAppearingAsync()
    {
        if (await authentication.IsAuthenticatedAsync())
            await navigation.GotoMainPageAsync();
    }

    [RelayCommand]
    private async Task LoginCommand()
        => await authentication.LoginAsync(Email, Password);

    [RelayCommand]
    private async Task GotoRegisterCommand()
        => await navigation.GotoRegisterPageAsync();
}