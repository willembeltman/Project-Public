using CommunityToolkit.Mvvm.Input;
using gAPI.Core.Interfaces;
using System.Collections.ObjectModel;
using UwvLlm.App.Core.Interfaces;
using UwvLlm.Shared.CrudInterfaces;
using UwvLlm.Shared.Dtos;

namespace UwvLlm.App.Core.ViewModels;

public partial class EmailViewModel(
    IDispatcherService dispatcher,
    IUserCrudService userService,
    IEmailService mailService,
    IClientConnection clientConnection,
    IUserNotificationCrudService userNotificationService,
    INavigationService navigationService,
    IUiService uiService) 
    : NotificationHubViewModel(dispatcher, clientConnection, userNotificationService, navigationService, uiService)
{
    public ObservableCollection<User> Users { get; } = [];
    public User? SelectedUser { get => field; set => SetProperty(ref field, value); }
    public string? Subject { get => field; set => SetProperty(ref field, value); }
    public string? Body { get => field; set => SetProperty(ref field, value); }

    public override async Task OnAppearingAsync()
    {
        var response = await userService.List(skip: 0, take: int.MaxValue, null, CancellationToken.None);
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

    [RelayCommand]
    public async Task Send()
        => await mailService.Send(SelectedUser?.Id, Subject, Body);
}