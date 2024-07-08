using LanCloud.Interfaces;
using System.Threading.Tasks;

namespace LanCloud.Client
{
    public class ClientService : IStarteble
    {
        public ClientService(App app)
        {
            App = app;
        }

        public App App { get; }

        public async Task StartAsync()
        {
            await Task.Run(() =>
            {
                while (!App.KillSwitch)
                {
                }
            });
        }
    }
}