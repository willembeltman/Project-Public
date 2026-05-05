using CommunityToolkit.Mvvm.Input;
using gAPI.Core.Interfaces;
using UwvLlm.App.Core.Interfaces;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.App.Core.ViewModels;

public partial class MainPageViewModel(
    IDispatcherService dispatcher,
    IClientConnection clientConnection,
    IUserNotificationCrudService userNotificationsService,
    INavigationService navigationService,
    IUiService uiService) 
    : NotificationHubViewModel(dispatcher, clientConnection, userNotificationsService, navigationService, uiService)
{
    [RelayCommand]
    public async Task SendEmail()
        => await NavigationService.GotoSendEmailPage();
}
