using gAPI.Generated;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UwvLlm.App.Interfaces;
using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.App.ViewModels;

public class NotificationHubViewModel 
    : BaseViewModel
    , INotificationHub
{
    private readonly IClientConnection ClientConnection;
    private readonly INotificationApi Notifications;

    public NotificationHubViewModel(
        IClientConnection clientConnection,
        INotificationApi notifications,
        INavigationService navigation)
    {
        ClientConnection = clientConnection;
        Notifications = notifications;
        OpenNotificationsCommand = new Command(async () => await navigation.OpenNotifications());
    }
    public async Task OnAppearingAsync()
    {
        await ClientConnection.SubscribeAsync(this);
        NotificationList = [.. await Notifications.GetNotificationList()];
    }
    public async Task OnDisappearingAsync()
    {
        await ClientConnection.UnsubscribeAsync(this);
    }

    public async Task NotificationReceived(NotificationDto notification)
    {
        NotificationList.Add(notification);
        NotificationCount = NotificationList.Count;
        HasNotifications = NotificationList.Count > 0; 
    }

    public int NotificationCount
    {
        get => field;
        set => SetProperty(ref field, value);
    }
    public bool HasNotifications
    {
        get => field;
        set => SetProperty(ref field, value);
    }
    public ObservableCollection<NotificationDto> NotificationList { get; private set; } = new();

    public ICommand OpenNotificationsCommand { get; }
}
