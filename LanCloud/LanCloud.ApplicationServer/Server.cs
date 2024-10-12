using LanCloud.ApplicationServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace LanCloud.ApplicationServer
{
    public class Server : IDisposable
    {
        private bool Disposed = false;
        private bool Listening = false;

        private List<ClientConnection> ActiveConnections;

        private IPEndPoint LocalEndPoint { get; }
        public IApplicationHandler ApplicationHandler { get; }
        private TcpListener Listener { get; }

        public Server(IPAddress ipAddress, int port, IApplicationHandler applicationHandler)
        {
            LocalEndPoint = new IPEndPoint(ipAddress, port);
            ApplicationHandler = applicationHandler;
            Listener = new TcpListener(LocalEndPoint);

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

                ClientConnection connection = new ClientConnection(client, ApplicationHandler);

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
