using System.Windows.Input;
using UwvLlm.App.Interfaces;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.App.ViewModels;

public class AppShellViewModel
{
    private readonly INotificationsApi Notifications;

    public AppShellViewModel(
        INotificationsApi notifications,
        INavigationService navigation)
    {
        Notifications = notifications;
        OpenNotificationsCommand = new Command(async () => await navigation.OpenNotifications());
    }
    public async Task OnAppearingAsync()
    {
        NotificationCount = await Notifications.GetNotificationCount();
    }

    public int NotificationCount { get; set; }
    public bool HasNotifications => NotificationCount > 0;

    public ICommand OpenNotificationsCommand { get; }
}
