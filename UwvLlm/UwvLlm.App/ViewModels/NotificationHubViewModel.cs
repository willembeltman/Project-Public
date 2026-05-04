using gAPI.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UwvLlm.App.Interfaces;
using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.App.ViewModels;

public partial class NotificationHubViewModel(
    IClientConnection clientConnection,
    INotificationApi notifications,
    INavigationService navigation)
    : BaseViewModel
    , INotificationHub
    , IDisposable
{
    private readonly CancellationTokenSource Cts = new();

    public async Task OnAppearingAsync()
    {
        clientConnection.SubscribeAsync(this);
        NotificationList.Clear();
        foreach (var notification in await notifications.GetNotificationList(Cts.Token))
            NotificationList.Add(notification);
    }

    public async Task OnDisappearingAsync() => clientConnection.UnsubscribeAsync(this);

    public async Task OnNotificationReceived(Notification notification) => MainThread.BeginInvokeOnMainThread(() =>
    {
        NotificationList.Add(notification);
        NotificationCount = NotificationList.Count;
        HasNotifications = NotificationList.Count > 0;
    });

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

    public ObservableCollection<Notification> NotificationList { get; private set; } = [];

    public ICommand OpenNotificationsCommand { get; } = new Command(async () => await navigation.OpenNotifications());

    public void Dispose()
    {
        Cts.Cancel();
        Cts.Dispose();
        GC.SuppressFinalize(this);
    }
}
