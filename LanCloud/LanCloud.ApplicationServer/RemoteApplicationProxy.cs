using LanCloud.Servers.Application;
using LanCloud.Shared.Dtos;
using LanCloud.Shared.Messages;
using LanCloud.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace LanCloud.Models
{
    public class RemoteApplicationProxy : IDisposable
    {
        public RemoteApplicationProxy(ServerConfig config)
        {
            if (config.IsThisComputer) throw new ArgumentNullException("geen verbinding naar jezelf opbouwen aub");

            Config = config;
            Thread = new Thread(new ThreadStart(Start));
            Thread.Start();
        }
        private ServerConfig Config { get; }
        private Thread Thread { get; }

        ConcurrentQueue<RemoteApplicationProxyQueueItem> Queue { get; } = new ConcurrentQueue<RemoteApplicationProxyQueueItem>();
        AutoResetEvent Enqueued { get; } = new AutoResetEvent(false);
        public bool Stop { get; set; }
        public bool Connected { get; private set; }
        public event EventHandler<EventArgs> StateChanged;

        public bool Ping()
        {
            try
            {
                return Send(ApplicationMessages.Ping) == "Pong";
            }
            catch
            {
                return false;
            }
        }
        public ShareDto[] GetExternalShareDtos()
        {

            try
            {
                var json = Send(ApplicationMessages.GetExternalShareDtos);
                var list = JsonConvert.DeserializeObject<ShareDto[]>(json);
                return list;
            }
            catch
            {
                return null;
            }
        }


        public string Send(ApplicationMessages message)
        {
            var requestItem = new RemoteApplicationProxyQueueItem(message);
            Queue.Enqueue(requestItem);
            Enqueued.Set();
            if (!requestItem.Done.WaitOne(10000))
                throw new Exception("Timeout occured");
            return requestItem.Response;
        }

        private void Start()
        {
            while (!Stop)
            {
                using (var client = new TcpClient(Config.Hostname, Config.Port))
                using (var stream = client.GetStream())
                using (var reader = new BinaryReader(stream))
                using (var writer = new BinaryWriter(stream))
                {
                    while (client.Connected)
                    {
                        if (!Connected)
                        {
                            Connected = true;
                            StateChanged?.Invoke(this, null);
                        }
                        if (Enqueued.WaitOne(1000))
                        {
                            while (Queue.TryDequeue(out RemoteApplicationProxyQueueItem requestItem))
                            {
                                writer.Write(Convert.ToInt32(requestItem.Message));
                                requestItem.Response = reader.ReadString();
                                requestItem.Done.Set();
                            }
                        }
                    }
                }
                if (Connected)
                {
                    Connected = false;
                    StateChanged?.Invoke(this, null);
                }

                Thread.Sleep(5000);
            }
        }

        public void Dispose()
        {
            Stop = true;
            if (Thread.CurrentThread != Thread) // Kan dat?
                Thread.Join();
        }
    }
}