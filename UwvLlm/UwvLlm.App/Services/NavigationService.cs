using UwvLlm.App.Interfaces;
using UwvLlm.App.Pages;

namespace UwvLlm.App.Services;

public class NavigationService : INavigationService
{
    public Task NavigateToAsync<TPage>() where TPage : Page
    {
        return Shell.Current.GoToAsync(typeof(TPage).Name);
    }
    public string GetPathAndQuery()
    {
        return Shell.Current.Title;
    }
    public Task OpenNotifications() => NavigateToAsync<NotificationsPage>();
    public Task GotoSendEmailPage() => NavigateToAsync<EmailPage>();
}