using System.Windows.Input;
using UwvLlm.App.Interfaces;

namespace UwvLlm.App.ViewModels;

public class EmailViewModel : BaseViewModel
{
    public EmailViewModel(IEmailService email)
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