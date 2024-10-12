using LanCloud.Servers.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace LanCloud.Servers.Application
{
    public class ApplicationServer : IDisposable
    {
        private bool Disposed = false;
        private bool Listening = false;

        private List<ApplicationServerClient> ActiveConnections;

        private IPEndPoint LocalEndPoint { get; }
        public IApplicationHandler ApplicationHandler { get; }
        private TcpListener Listener { get; }

        public ApplicationServer(IPAddress ipAddress, int port, IApplicationHandler applicationHandler)
        {
            LocalEndPoint = new IPEndPoint(ipAddress, port);
            ApplicationHandler = applicationHandler;
            Listener = new TcpListener(LocalEndPoint);

            Listening = true;
            Listener.Start();

            ActiveConnections = new List<ApplicationServerClient>();

            Listener.BeginAcceptTcpClient(HandleAcceptTcpClient, Listener);
        }

        private void HandleAcceptTcpClient(IAsyncResult result)
        {
            if (Listening)
            {
                Listener.BeginAcceptTcpClient(HandleAcceptTcpClient, Listener);

                TcpClient client = Listener.EndAcceptTcpClient(result);

                ApplicationServerClient connection = new ApplicationServerClient(client, ApplicationHandler);

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

                    foreach (ApplicationServerClient conn in ActiveConnections)
                    {
                        conn.Dispose();
                    }
                }
            }

            Disposed = true;
        }
    }
}
