using UwvLlm.App.Core.ViewModels;

namespace UwvLlm.App.Pages;

public partial class NotificationsPage : ContentPage
{
    private readonly NotificationPageViewModel ViewModel;

    public NotificationsPage(NotificationPageViewModel viewModel)
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

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();
        await ViewModel.OnDisappearingAsync();
    }
}