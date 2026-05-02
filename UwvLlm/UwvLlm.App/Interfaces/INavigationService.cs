using gAPI.Core.Client;

namespace UwvLlm.App.Interfaces;

public interface INavigationService : INavigationManager
{
    Task NavigateToAsync<TPage>() where TPage : Page;
    Task GotoSendEmailPage();
    Task OpenNotifications();
    Task GotoMainPageAsync();
    Task GotoRegisterPageAsync();
    Task GotoLoginPageAsync();
}