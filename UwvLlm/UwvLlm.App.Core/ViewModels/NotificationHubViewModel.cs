using gAPI.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UwvLlm.App.Core.Interfaces;
using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Interfaces;

#pragma warning disable IDE0290 // Use primary constructor
namespace UwvLlm.App.Core.ViewModels;

public partial class NotificationHubViewModel : BaseViewModel
    , INotificationHub
    , IDisposable
{
    protected readonly CancellationTokenSource Cts = new();
    protected readonly IDispatcherService Dispatcher;
    protected readonly IClientConnection ClientConnection;
    protected readonly IUserNotificationsService UserNotificationsService;
    protected readonly INavigationService NavigationService;
    protected readonly IUiService UiService;

    public NotificationHubViewModel(
        IDispatcherService dispatcher,
        IClientConnection clientConnection,
        IUserNotificationsService userNotificationsService,
        INavigationService navigationService,
        IUiService uiService)
    {
        Dispatcher = dispatcher;
        ClientConnection = clientConnection;
        UserNotificationsService = userNotificationsService;
        NavigationService = navigationService;
        UiService = uiService;
        OpenNotificationsCommand = new RelayCommand(async () => await navigationService.OpenNotifications());
    }

    public virtual async Task OnAppearingAsync()
    {
        ClientConnection.SubscribeAsync(this);

        NotificationList.Clear();
        var response = await UserNotificationsService.List(0, int.MaxValue, null, Cts.Token);
        if (response.Success == false || response.Response == null)
        {
            await UiService.ShowAlert("Cannot load users", "There is a problem while loading the users", "OK");
            return;
        }

        await foreach (var notification in response.Response)
            NotificationList.Add(notification);
    }

    public virtual async Task OnDisappearingAsync() => ClientConnection.UnsubscribeAsync(this);

    public virtual async Task OnNotificationReceived(UserNotification notification)
        => Dispatcher.Invoke(() =>
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

    public ObservableCollection<UserNotification> NotificationList { get; } = [];

    public ICommand OpenNotificationsCommand { get; }

    public void Dispose()
    {
        Cts.Cancel();
        Cts.Dispose();
        GC.SuppressFinalize(this);
    }
}
