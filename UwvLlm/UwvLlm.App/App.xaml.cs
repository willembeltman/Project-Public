using UwvLlm.App.ViewModels;

namespace UwvLlm.App;

public partial class App : Application
{
    private readonly AppShellViewModel Vm;
    public App(AppShellViewModel vm)
    {
        Vm = vm;
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell(Vm));
    }
}