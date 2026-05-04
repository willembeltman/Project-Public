using CommunityToolkit.Mvvm.Input;
using UwvLlm.App.Core.Interfaces;
using UwvLlm.App.Core.Services;

namespace UwvLlm.App.Core.ViewModels;

public partial class RegisterViewModel(
    INavigationService navigationService,
    AuthenticationService authenticationService) : BaseViewModel
{
    public string? UserName { get => field; set => SetProperty(ref field, value); }
    public string? Email { get => field; set => SetProperty(ref field, value); }
    public string? Password { get => field; set => SetProperty(ref field, value); }
    public string? PasswordRepeat { get => field; set => SetProperty(ref field, value); }

    [RelayCommand]
    public async Task Register() 
        => await authenticationService.RegisterAsync(UserName, Email, Password, PasswordRepeat);

    [RelayCommand]
    public async Task GotoLogin()
        => await navigationService.GotoLoginPageAsync();
}