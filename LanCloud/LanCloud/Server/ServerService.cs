using LanCloud.Interfaces;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace LanCloud.Server
{
    public class ServerService : IStarteble
    {
        public ServerService(App app)
        {
            App = app;
        }

        App App { get; }

        public async Task StartAsync()
        {
            await Task.Run(() =>
            {
                TcpListener listener = new TcpListener(App.Settings.IPAddress, App.Settings.Port);
                listener.Start();
                while (!App.KillSwitch && (listener.Server.Connected || listener.Pending()))
                {
                    using (var tcpclient = listener.AcceptTcpClient())
                    {
                    }
                }
            });
        }
    }
}