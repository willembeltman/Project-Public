using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace LanCloud.FtpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandHandler = new CommandHandler(@"D:\Willem\Videos");
            using (FtpServer server = new FtpServer(IPAddress.Any, 21, commandHandler))
            {
                server.Start();

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey(true);
            }
        }
    }
}
