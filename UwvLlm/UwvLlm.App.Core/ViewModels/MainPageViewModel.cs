using gAPI.Interfaces;
using System.Windows.Input;
using UwvLlm.App.Core.Interfaces;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.App.Core.ViewModels;

public class MainPageViewModel(
        IDispatcherService dispatcher,
    IClientConnection clientConnection,
    IUserNotificationCrudService userNotificationsService,
    INavigationService navigationService,
    IUiService uiService) 
    : NotificationHubViewModel(dispatcher, clientConnection, userNotificationsService, navigationService, uiService)
{
    public ICommand SendEmailCommand { get; } = new RelayCommand(async () => await navigationService.GotoSendEmailPage());
}
