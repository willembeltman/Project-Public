using gAPI.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UwvLlm.App.Interfaces;
using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.App.ViewModels;

public class EmailViewModel : NotificationHubViewModel
{
    private readonly IUsersService UsersService;

    public EmailViewModel(
        IUsersService userService,
        IUiService uiService,
        IClientConnection clientConnection,
        INotificationApi notificationApi,
        INavigationService navigationService,
        IMailService mailService)
        : base(clientConnection, notificationApi, navigationService)
    {
        UsersService = userService;
        SendCommand = new Command(async () => await mailService.Send(To, Subject, Body));
    }

    public override async Task OnAppearingAsync()
    {
        // Let op we doen niets met paginarisering?
        var response = await UsersService.List(skip: 0, take: int.MaxValue, null, CancellationToken.None);
        if (response.Success == false || response.Response == null)
        {
            return;
        }

        Users.Clear();
        await foreach (var notification in response.Response)
            Users.Add(notification);

        await base.OnAppearingAsync();
    }

    public ObservableCollection<User> Users { get; private set; } = [];

    public Guid? To
    {
        get => field;
        set => SetProperty(ref field, value);
    }

    public User? SelectedUser
    {
        get => field;
        set
        {
            if (SetProperty(ref field, value))
            {
                To = value?.Id; // sync naar je bestaande property
            }
        }
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