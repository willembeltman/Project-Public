using UwvLlm.App.ViewModels;

namespace UwvLlm.App.Pages;

public partial class EmailPage : ContentPage
{
    public EmailPage(EmailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}