namespace UwvLlm.App.Interfaces;

public interface IUiService
{
    Task ShowAlert(string title, string message, string cancel);
}
