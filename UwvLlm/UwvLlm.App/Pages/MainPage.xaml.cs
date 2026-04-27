using UwvLlm.App.ViewModels;

namespace UwvLlm.App.Pages;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
