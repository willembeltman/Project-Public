using System.Windows.Input;
using UwvLlm.App.Interfaces;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.App.ViewModels;

public class MainPageViewModel : BaseNotificationsViewModel
{
    public MainPageViewModel(
        INotificationsApi notifications,
        INavigationService navigation)
        : base(notifications, navigation)
    {
        GotoSendEmailPageCommand = new Command(async () => await navigation.GotoSendEmailPage());
    }

    public ICommand GotoSendEmailPageCommand { get; }
}
