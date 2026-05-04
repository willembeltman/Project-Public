using gAPI.Interfaces;
using UwvLlm.App.Core.Interfaces;
using UwvLlm.Shared.Interfaces;

#pragma warning disable IDE0290 // Use primary constructor
namespace UwvLlm.App.Core.ViewModels;

public class NotificationPageViewModel : NotificationHubViewModel
{
    public NotificationPageViewModel(
        IDispatcherService dispatcher,
        IClientConnection clientConnection, 
        IUserNotificationsService userNotificationsService, 
        INavigationService navigationService,
        IUiService uiService) 
        : base(dispatcher, clientConnection, userNotificationsService, navigationService, uiService)
    {
    }
}
