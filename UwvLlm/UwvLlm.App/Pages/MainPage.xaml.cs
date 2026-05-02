using UwvLlm.App.ViewModels;

namespace UwvLlm.App.Pages;

public partial class MainPage : ContentPage
{
    private readonly MainPageViewModel ViewModel;

    public MainPage(MainPageViewModel viewModel)
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
