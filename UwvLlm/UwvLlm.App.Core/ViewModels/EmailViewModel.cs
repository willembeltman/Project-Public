using gAPI.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UwvLlm.App.Core.Interfaces;
using UwvLlm.App.Core.Services;
using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.App.Core.ViewModels;

public class EmailViewModel : NotificationHubViewModel
{
    private readonly IUserCrudService UsersService;
    private readonly MailService MailService;

    public EmailViewModel(
        IDispatcherService dispatcher,
        IUserCrudService userService,
        MailService mailService,
        IClientConnection clientConnection,
        IUserNotificationsService userNotificationService,
        INavigationService navigationService,
        IUiService uiService)
        : base(dispatcher, clientConnection, userNotificationService, navigationService, uiService)
    {
        UsersService = userService;
        MailService = mailService;
        SendCommand = new RelayCommand(async () => await MailService.Send(SelectedUser?.Id, Subject, Body));
    }

    public override async Task OnAppearingAsync()
    {
        var response = await UsersService.List(skip: 0, take: int.MaxValue, null, CancellationToken.None);
        if (response.Success == false || response.Response == null)
        {
            await UiService.ShowAlert("Cannot load users", "There is a problem while loading the users", "OK");
            return;
        }

        Users.Clear();
        await foreach (var notification in response.Response)
            Users.Add(notification);

        await base.OnAppearingAsync();
    }

    public ICommand SendCommand { get; }
    public ObservableCollection<User> Users { get; } = [];

    public User? SelectedUser
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
}