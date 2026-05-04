namespace UwvLlm.App.Core.Interfaces;

public interface IUiService
{
    Task ShowAlert(string title, string message, string cancel);
}
