using LanCloud.Servers.Ftp.Interfaces;
using LanCloud.Logger;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace LanCloud.Servers.Ftp
{
    public class FtpServer : IDisposable
    {
        ILogger Logger = LogManager.GetLogger(typeof(FtpServer));

        private bool Disposed = false;
        private bool Listening = false;

        private List<ClientConnection> ActiveConnections;

        private IPEndPoint LocalEndPoint { get; }
        public IFtpHandler CommandHandler { get; }
        private TcpListener Listener { get; }

        public FtpServer(IPAddress ipAddress, int port, IFtpHandler commandHandler)
        {
            LocalEndPoint = new IPEndPoint(ipAddress, port);
            CommandHandler = commandHandler;
            Listener = new TcpListener(LocalEndPoint);

            Logger.Info("#Version: 1.0");
            Logger.Info("#Fields: date time c-ip c-port cs-username cs-method cs-uri-stem sc-status sc-bytes cs-bytes s-name s-port");

            Listening = true;
            Listener.Start();

            ActiveConnections = new List<ClientConnection>();

            Listener.BeginAcceptTcpClient(HandleAcceptTcpClient, Listener);
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
