using UwvLlm.App.ViewModels;

namespace UwvLlm.App.Pages;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}