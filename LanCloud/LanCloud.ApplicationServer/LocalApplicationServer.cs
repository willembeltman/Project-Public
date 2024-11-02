using LanCloud.LogHelper;
using LanCloud.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace LanCloud.Servers.Application
{
    public class LocalApplicationServer : IDisposable
    {
        private bool Disposed = false;
        private bool Listening = false;

        private List<LocalApplicationServerConnection> ActiveConnections;

        private IPEndPoint LocalEndPoint { get; }
        public ILocalApplicationHandler ApplicationHandler { get; }
        private TcpListener Listener { get; }
        public ILogger Logger { get; }

        public LocalApplicationServer(
            IPAddress ipAddress,
            int port, 
            ILocalApplicationHandler applicationHandler, 
            ILogger logger)
        {
            LocalEndPoint = new IPEndPoint(ipAddress, port);
            ApplicationHandler = applicationHandler;
            Listener = new TcpListener(LocalEndPoint);

            Listening = true;
            Listener.Start();

            ActiveConnections = new List<LocalApplicationServerConnection>();

            Listener.BeginAcceptTcpClient(HandleAcceptTcpClient, Listener);
            Logger = logger;
        }

        private void HandleAcceptTcpClient(IAsyncResult result)
        {
            if (Listening)
            {
                Listener.BeginAcceptTcpClient(HandleAcceptTcpClient, Listener);

                TcpClient client = Listener.EndAcceptTcpClient(result);

                LocalApplicationServerConnection connection = new LocalApplicationServerConnection(client, ApplicationHandler);

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
                    Listening = false;
                    Listener.Stop();

                    foreach (LocalApplicationServerConnection conn in ActiveConnections)
                    {
                        conn.Dispose();
                    }
                }
            }

            Disposed = true;
        }
    }
}
