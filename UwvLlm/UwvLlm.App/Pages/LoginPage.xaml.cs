using UwvLlm.App.ViewModels;

namespace UwvLlm.App.Pages;

public partial class LoginPage : ContentPage
{
    private readonly LoginViewModel ViewModel;

    public LoginPage(LoginViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.OnAppearingAsync();
    }
}