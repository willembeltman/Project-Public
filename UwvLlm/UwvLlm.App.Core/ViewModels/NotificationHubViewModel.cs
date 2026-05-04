using gAPI.Interfaces;
using System.Collections.ObjectModel;
using UwvLlm.App.Core.Interfaces;
using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Interfaces;
using CommunityToolkit.Mvvm.Input;

namespace UwvLlm.App.Core.ViewModels;

public partial class NotificationHubViewModel(
    IDispatcherService dispatcher,
    IClientConnection clientConnection,
    IUserNotificationCrudService userNotificationsService,
    INavigationService navigationService,
    IUiService uiService)
    : BaseViewModel
    , INotificationHub
    , IDisposable
{
    protected readonly CancellationTokenSource Cts = new();
    protected readonly IDispatcherService Dispatcher = dispatcher;
    protected readonly IClientConnection ClientConnection = clientConnection;
    protected readonly IUserNotificationCrudService UserNotificationsService = userNotificationsService;
    protected readonly INavigationService NavigationService = navigationService;
    protected readonly IUiService UiService = uiService;

    public int NotificationCount { get => field; set => SetProperty(ref field, value); }

    public bool HasNotifications { get => field; set => SetProperty(ref field, value); }

    public ObservableCollection<UserNotification> NotificationList { get; } = [];

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

    public virtual async Task OnDisappearingAsync()
        => ClientConnection.UnsubscribeAsync(this);

    public virtual async Task OnNotificationReceived(UserNotification notification)
        => Dispatcher.Invoke(() =>
        {
            NotificationList.Add(notification);
            NotificationCount = NotificationList.Count;
            HasNotifications = NotificationList.Count > 0;
        });

    [RelayCommand]
    private async Task OpenNotificationsCommand()
        => await NavigationService.OpenNotifications();

    public void Dispose()
    {
        Cts.Cancel();
        Cts.Dispose();
        GC.SuppressFinalize(this);
    }
}
