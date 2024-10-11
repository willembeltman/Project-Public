using System;
using System.Net.Sockets;

namespace SharpFtpServer
{
    public class DataConnectionOperation
    {
        public Func<NetworkStream, string, string> Operation { get; set; }
        public string Arguments { get; set; }
    }
}