using UwvLlm.App.Interfaces;

namespace UwvLlm.App.Services;

public class UiService : IUiService
{
    public Task ShowAlert(string title, string message, string cancel)
    {
        return Shell.Current.DisplayAlertAsync(title, message, cancel);
    }
}