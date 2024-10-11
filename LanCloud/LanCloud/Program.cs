using System.Threading.Tasks;
using System.IO;
using System;

namespace LanCloud
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //string prefix = "http://localhost:8080/";
            //string baseFolder = @"D:\Willem\Videos"; // Vervang dit met je map
            //var server = new SimpleHttpServer(prefix, baseFolder);
            //server.Start();

            string ipAddress = "127.0.0.1";
            int port = 21;
            string baseDirectory = @"D:\Willem\Videos";

            if (!Directory.Exists(baseDirectory))
            {
                Console.WriteLine($"De directory {baseDirectory} bestaat niet.");
                return;
            }

            var server = new SimpleFTPServer(ipAddress, port, baseDirectory);
            await server.StartAsync();
        }
    }
}



