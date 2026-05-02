using gAPI.Generated;
using System.Windows.Input;
using UwvLlm.App.Interfaces;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.App.ViewModels;

public class MainPageViewModel(
    IClientConnection clientConnection,
    INotificationApi notifications,
    INavigationService navigation) 
    : NotificationHubViewModel(clientConnection, notifications, navigation)
{
    public ICommand GotoSendEmailPageCommand { get; } = new Command(async () => await navigation.GotoSendEmailPage());
}
