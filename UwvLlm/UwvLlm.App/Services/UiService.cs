using UwvLlm.App.Interfaces;

namespace UwvLlm.App.Services;

public class UiService(IServiceProvider services) : IUiService
{
    private static Page GetCurrentPage()
    {
        var page =
            Application.Current != null && 
            Application.Current.Windows.Count > 0 &&
            Application.Current?.Windows[0].Page != null
            ? Application.Current?.Windows[0].Page!
            : throw new InvalidOperationException("No active page found");
        return page;
    }

    public Task ShowAlert(string title, string message, string cancel)
    {
        return GetCurrentPage().DisplayAlertAsync(title, message, cancel);
    }

    public Task NavigateToAsync<TPage>() where TPage : Page
    {
        var page = services.GetService<TPage>()
                   ?? throw new InvalidOperationException($"Page {typeof(TPage)} not registered");

        return GetCurrentPage().Navigation.PushAsync(page);
    }

    public string GetPathAndQuery()
        => GetCurrentPage().Title;
}

