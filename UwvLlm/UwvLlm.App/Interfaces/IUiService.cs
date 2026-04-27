using gAPI.Core.Client;

namespace UwvLlm.App.Interfaces;

public interface IUiService : INavigationManager
{
    Task ShowAlert(string title, string message, string cancel);
    Task NavigateToAsync<TPage>() where TPage : Page;
}