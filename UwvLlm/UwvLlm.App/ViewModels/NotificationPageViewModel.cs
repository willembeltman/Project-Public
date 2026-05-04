using gAPI.Interfaces;
using UwvLlm.App.Interfaces;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.App.ViewModels;

public class NotificationPageViewModel : NotificationHubViewModel
{
    public NotificationPageViewModel(
        IClientConnection clientConnection, 
        IUserNotificationsService userNotificationsService, 
        INavigationService navigationService,
        IUiService uiService) 
        : base(clientConnection, userNotificationsService, navigationService, uiService)
    {
    }
}
