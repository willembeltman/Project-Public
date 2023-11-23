using System;
using System.Net.Sockets;
using System.Threading;

namespace LanCloud
{
    public class Server
    {
        public Server(App app)
        {
            App = app;
        }

        App App { get; }

        public void Start()
        {
            TcpListener listener = new TcpListener(App.Settings.IPAddress, App.Settings.Port);
            listener.Start();
            while (listener.Server.Connected || listener.Pending())
            {
                var tcpclient = listener.AcceptTcpClient();

            }
        }

        public void SettingsChanged()
        {
        }
    }
}