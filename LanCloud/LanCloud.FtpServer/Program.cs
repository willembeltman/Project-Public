using System;
using System.Net;
using LanCloud.FtpServer;

namespace LanCloud.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandHandler = new FtpCommandHandler(@"E:\LanCloud");
            using (Server server = new Server(IPAddress.Any, 21, commandHandler))
            {
                server.Start();

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey(true);
            }
        }
    }
}
