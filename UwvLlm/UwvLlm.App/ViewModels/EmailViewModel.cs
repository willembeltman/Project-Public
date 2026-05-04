using gAPI.Interfaces;
using System.Windows.Input;
using UwvLlm.App.Interfaces;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.App.ViewModels;

public class EmailViewModel : NotificationHubViewModel
{
    public EmailViewModel(
        IClientConnection clientConnection,
        INotificationApi notifications,
        INavigationService navigation,
        IEmailService email)
        : base(clientConnection, notifications, navigation)
    {
        SendCommand = new Command(async () => await email.Send(From, To, Subject, Body));
    }

    public string? From
    {
        get => field;
        set => SetProperty(ref field, value);
    }

    public string? To
    {
        get => field;
        set => SetProperty(ref field, value);
    }

    public string? Subject
    {
        get => field;
        set => SetProperty(ref field, value);
    }

    public string? Body
    {
        get => field;
        set => SetProperty(ref field, value);
    }

    public ICommand SendCommand { get; }
}