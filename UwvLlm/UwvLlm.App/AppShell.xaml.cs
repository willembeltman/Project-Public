using UwvLlm.App.Pages;
using UwvLlm.App.ViewModels;

namespace UwvLlm.App;

public partial class AppShell : Shell
{
    private readonly AppShellViewModel Vm;
    public AppShell(AppShellViewModel vm)
    {
        Vm = vm;
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Vm.OnAppearingAsync();
    }
}
