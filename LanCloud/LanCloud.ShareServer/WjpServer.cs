using LanCloud.Shared.Log;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace LanCloud.Servers.Wjp
{
    public class WjpServer : IDisposable
    {
        private bool Disposed = false;
        private bool Listening = false;

        private IPEndPoint LocalEndPoint { get; }
        public IWjpHandler ShareHandler { get; }
        public ILogger Logger { get; }
        private TcpListener Listener { get; }
        private List<WjpClientConnection> ActiveConnections { get; } = new List<WjpClientConnection>();

        public WjpServer(IPAddress ipAddress, int port, IWjpHandler applicationHandler, ILogger logger)
        {
            LocalEndPoint = new IPEndPoint(ipAddress, port);
            ShareHandler = applicationHandler;
            Logger = logger;

            Listener = new TcpListener(LocalEndPoint);

            Listening = true;
            Listener.Start();

            Listener.BeginAcceptTcpClient(HandleAcceptTcpClient, Listener);

            //Logger.Info($"Loaded");
        }

        private void HandleAcceptTcpClient(IAsyncResult result)
        {
            if (Listening)
            {
                Listener.BeginAcceptTcpClient(HandleAcceptTcpClient, Listener);

                var client = Listener.EndAcceptTcpClient(result);

                var connection = new WjpClientConnection(this, client, ShareHandler);
                connection.Start();
            }
        }
        public void AddConnection(WjpClientConnection connection)
        {
            lock(this)
                ActiveConnections.Add(connection);
        }
        public void RemoveConnection(WjpClientConnection connection)
        {
            lock (this) 
                ActiveConnections.Remove(connection);
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

                    foreach (WjpClientConnection conn in ActiveConnections)
                    {
                        conn.Dispose();
                    }
                }
            }

            Disposed = true;
        }
    }
}
