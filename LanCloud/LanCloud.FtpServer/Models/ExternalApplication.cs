﻿using LanCloud.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace LanCloud.Models
{
    internal class ExternalApplication : IDisposable
    {
        public ExternalApplication(ServerConfig config)
        {
            if (config.IsThisComputer) throw new ArgumentNullException("geen verbinding naar jezelf opbouwen aub");

            Config = config;
            Thread = new Thread(new ThreadStart(Start));
            Thread.Start();
        }
        private ServerConfig Config { get; }
        private Thread Thread { get; }

        ConcurrentQueue<RequestItem> Requests { get; } = new ConcurrentQueue<RequestItem>();
        AutoResetEvent NewRequest { get; } = new AutoResetEvent(false);
        public bool Stop { get; set; }
        public bool Connected { get; private set; }
        public event EventHandler<EventArgs> StateChanged;

        public bool Ping()
        {
            try
            {
                return Send("Ping") == "Pong";
            }
            catch
            {
                return false;
            }
        }
        public ExternalFtpDto[] GetExternalFtps()
        {

            try
            {
                var json = Send("GetOwnFtpsAsExternalFtps");
                var list = JsonConvert.DeserializeObject<ExternalFtpDto[]>(json);
                return list;
            }
            catch
            {
                return null;
            }
        }


        public string Send(string request)
        {
            var requestItem = new RequestItem()
            {
                Request = request
            };
            Requests.Enqueue(requestItem);
            NewRequest.Set();
            if (!requestItem.RequestDone.WaitOne(10000))
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
                        if (NewRequest.WaitOne(1000))
                        {
                            while (Requests.TryDequeue(out RequestItem requestItem))
                            {
                                writer.Write(requestItem.Request);
                                requestItem.Response = reader.ReadString();
                                requestItem.RequestDone.Set();
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

        class RequestItem
        {
            public AutoResetEvent RequestDone { get; } = new AutoResetEvent(false);
            public string Request { get; set; }
            public string Response { get; set; }
        }
    }

}