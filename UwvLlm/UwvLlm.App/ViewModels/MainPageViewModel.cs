using System.Windows.Input;
using UwvLlm.App.Interfaces;

namespace UwvLlm.App.ViewModels;

public class MainPageViewModel
{
    public MainPageViewModel(
        INavigationService navigation)
    {
        GotoSendEmailPageCommand = new Command(async () => await navigation.GotoSendEmailPage());
    }

    public ICommand GotoSendEmailPageCommand { get; }
}
