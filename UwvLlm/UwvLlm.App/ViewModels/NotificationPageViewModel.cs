using gAPI.Generated;
using UwvLlm.App.Interfaces;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.App.ViewModels;

public class NotificationPageViewModel : NotificationHubViewModel
{
    public NotificationPageViewModel(
        IClientConnection clientConnection, 
        INotificationApi notifications, 
        INavigationService navigation) 
        : base(clientConnection, notifications, navigation)
    {
    }
}
