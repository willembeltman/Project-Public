using UwvLlm.App.Core.Interfaces;
using UwvLlm.App.Pages;

namespace UwvLlm.App.Services;

public class NavigationService : INavigationService
{
    public Task NavigateToAsync<TPage>()
    {
        return Shell.Current.GoToAsync(typeof(TPage).Name);
    }
    public string GetPathAndQuery()
    {
        return Shell.Current.Title;
    }
    public Task GotoMainPageAsync() => NavigateToAsync<MainPage>();
    public Task GotoRegisterPageAsync() => NavigateToAsync<RegisterPage>();
    public Task GotoLoginPageAsync() => NavigateToAsync<LoginPage>();
    public Task OpenNotifications() => NavigateToAsync<NotificationsPage>();
    public Task GotoSendEmailPage() => NavigateToAsync<EmailPage>();
}