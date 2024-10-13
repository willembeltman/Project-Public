using LanCloud.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace LanCloud.Servers.Share
{
    public class LocalShareServer : IDisposable
    {
        private bool Disposed = false;
        private bool Listening = false;

        private List<LocalShareServerConnection> ActiveConnections;

        private IPEndPoint LocalEndPoint { get; }
        public ILocalShareHandler ShareHandler { get; }
        private TcpListener Listener { get; }

        public LocalShareServer(IPAddress ipAddress, int port, ILocalShareHandler applicationHandler)
        {
            LocalEndPoint = new IPEndPoint(ipAddress, port);
            ShareHandler = applicationHandler;
            Listener = new TcpListener(LocalEndPoint);

            Listening = true;
            Listener.Start();

            ActiveConnections = new List<LocalShareServerConnection>();

            Listener.BeginAcceptTcpClient(HandleAcceptTcpClient, Listener);
        }

        private void HandleAcceptTcpClient(IAsyncResult result)
        {
            if (Listening)
            {
                Listener.BeginAcceptTcpClient(HandleAcceptTcpClient, Listener);

                TcpClient client = Listener.EndAcceptTcpClient(result);

                LocalShareServerConnection connection = new LocalShareServerConnection(client, ShareHandler);

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

                    foreach (LocalShareServerConnection conn in ActiveConnections)
                    {
                        conn.Dispose();
                    }
                }
            }

            Disposed = true;
        }
    }
}
