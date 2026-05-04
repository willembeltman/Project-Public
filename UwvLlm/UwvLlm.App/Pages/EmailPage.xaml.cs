using UwvLlm.App.Core.ViewModels;

namespace UwvLlm.App.Pages;

public partial class EmailPage : ContentPage
{
    private readonly EmailViewModel ViewModel;

    public EmailPage(EmailViewModel viewModel)
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