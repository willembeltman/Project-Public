using gAPI.Interfaces;
using System.Windows.Input;
using UwvLlm.App.Interfaces;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.App.ViewModels;

public class MainPageViewModel(
    IClientConnection clientConnection,
    IUserNotificationsService userNotificationsService,
    INavigationService navigationService,
    IUiService uiService) 
    : NotificationHubViewModel(clientConnection, userNotificationsService, navigationService, uiService)
{
    public ICommand SendEmailCommand { get; } = new Command(async () => await navigationService.GotoSendEmailPage());
}
