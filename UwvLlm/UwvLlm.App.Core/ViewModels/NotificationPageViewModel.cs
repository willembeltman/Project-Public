using gAPI.Interfaces;
using UwvLlm.App.Core.Interfaces;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.App.Core.ViewModels;

public class NotificationPageViewModel(
    IDispatcherService dispatcher,
    IClientConnection clientConnection,
    IUserNotificationCrudService userNotificationsService,
    INavigationService navigationService,
    IUiService uiService) 
    : NotificationHubViewModel(dispatcher, clientConnection, userNotificationsService, navigationService, uiService)
{
}
