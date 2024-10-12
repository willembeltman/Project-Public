using System;
using System.Net.Sockets;

namespace LanCloud.FtpServer
{
    public class DataConnectionOperation
    {
        public Func<NetworkStream, string, string> Operation { get; set; }
        public string Arguments { get; set; }
    }
}