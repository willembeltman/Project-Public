using gAPI.Generated;
using UwvLlm.App.Interfaces;

namespace UwvLlm.App.Services;

public class NotificationsService(
    IClientConnection clientConnection)
    : INotificationsService
    , IAsyncDisposable
{
    public async Task InitializeAsync() // Todo aanroepen?
    {
        await clientConnection.SubscribeAsync(this); // this as IAppHub
    }
    public Task ShowEmailReceived(string id, string text)
    {
        // Todo
        throw new NotImplementedException();
    }
    public async ValueTask DisposeAsync()
    {
        await clientConnection.UnsubscribeAsync(this);
    }

}
