using LanCloud.Servers.Ftp.Interfaces;
using LanCloud.Shared.Log;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace LanCloud.Servers.Ftp
{
    public class FtpServer : IDisposable
    {
        private bool Disposed = false;
        private bool Listening = false;

        private List<ClientConnection> ActiveConnections;

        private IPEndPoint LocalEndPoint { get; }
        public IFtpHandler CommandHandler { get; }
        private TcpListener Listener { get; }
        public ILogger Logger { get; }

        public FtpServer(IPAddress ipAddress, int port, IFtpHandler commandHandler, ILogger logger)
        {
            LocalEndPoint = new IPEndPoint(ipAddress, port);
            CommandHandler = commandHandler;
            Logger = logger;
            Listener = new TcpListener(LocalEndPoint);

            Listening = true;
            Listener.Start();

            ActiveConnections = new List<ClientConnection>();

            Listener.BeginAcceptTcpClient(HandleAcceptTcpClient, Listener);

            Logger.Info("Virtual Ftp Server started");
        }

        private void HandleAcceptTcpClient(IAsyncResult result)
        {
            if (Listening)
            {
                Listener.BeginAcceptTcpClient(HandleAcceptTcpClient, Listener);

                TcpClient client = Listener.EndAcceptTcpClient(result);

                ClientConnection connection = new ClientConnection(client, CommandHandler, Logger);

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
                    Logger.Info("Stopping FtpServer");

                    Listening = false;
                    Listener.Stop();

                    foreach (ClientConnection conn in ActiveConnections)
                    {
                        conn.Dispose();
                    }
                }
            }

            Disposed = true;
        }
    }
}
