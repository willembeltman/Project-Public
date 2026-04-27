using System.Windows.Input;
using UwvLlm.App.Interfaces;

namespace UwvLlm.App.ViewModels;

public class EmailViewModel : BaseViewModel
{
    private readonly IUiService Ui;


    public EmailViewModel(IUiService ui)
    {
        Ui = ui;
        SendCommand = new Command(async () => await Send());
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

    private async Task Send()
    {
        await Ui.ShowAlert("OK", $"Mail naar {To}", "OK");
    }
}