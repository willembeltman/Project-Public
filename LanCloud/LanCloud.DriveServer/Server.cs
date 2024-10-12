using LanCloud.FtpServer.Interfaces;
using LanCloud.Logger;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace LanCloud.FtpServer
{
    public class Server : IDisposable
    {
        ILogger Logger = LogManager.GetLogger(typeof(Server));

        private bool Disposed = false;
        private bool Listening = false;

        private TcpListener Listener;
        private List<ClientConnection> ActiveConnections;

        private IPEndPoint LocalEndPoint { get; }
        public IFtpCommandHandler CommandHandler { get; }

        public Server(IPAddress ipAddress, int port, IFtpCommandHandler commandHandler)
        {
            LocalEndPoint = new IPEndPoint(ipAddress, port);
            CommandHandler = commandHandler;
        }

        public void Start()
        {
            Listener = new TcpListener(LocalEndPoint);

            Logger.Info("#Version: 1.0");
            Logger.Info("#Fields: date time c-ip c-port cs-username cs-method cs-uri-stem sc-status sc-bytes cs-bytes s-name s-port");

            Listening = true;
            Listener.Start();

            ActiveConnections = new List<ClientConnection>();

            Listener.BeginAcceptTcpClient(HandleAcceptTcpClient, Listener);
        }

        public void Stop()
        {
            Logger.Info("Stopping FtpServer");

            Listening = false;
            Listener.Stop();

            Listener = null;
        }

        private void HandleAcceptTcpClient(IAsyncResult result)
        {
            if (Listening)
            {
                Listener.BeginAcceptTcpClient(HandleAcceptTcpClient, Listener);

                TcpClient client = Listener.EndAcceptTcpClient(result);

                ClientConnection connection = new ClientConnection(client, CommandHandler);

                ActiveConnections.Add(connection);

                ThreadPool.QueueUserWorkItem(connection.HandleClient, client);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    Stop();

                    foreach (ClientConnection conn in ActiveConnections)
                    {
                        conn.Dispose();
                    }

                    LogManager.DisposeLoggers();
                }
            }

            Disposed = true;
        }
    }
}
