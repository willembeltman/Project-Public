using Microsoft.AspNetCore.SignalR;

namespace BlazorSignalR.Server.Hubs
{
    public class AppHub : Hub
    {
        public async Task SendMessage(string user, string text)
        {
            await ReceiveMessage(user, text);
        }
        public async Task ReceiveMessage(string user, string text)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, text);
        }
    }
}
