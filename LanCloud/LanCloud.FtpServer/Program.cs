using System;
using LanCloud.Services;
using LanCloud.Collections;
using LanCloud.Domain.Application;
using LanCloud.Domain.VirtualFtp;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace LanCloud
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentDirectory = Environment.CurrentDirectory;
            var logService = new LogService(currentDirectory);
            using (var logger = logService.Create())
            {
                var configService = new ConfigService(currentDirectory, logger);
                var config = configService.Load();

                using (var remoteApplications = new RemoteApplicationCollection(config, logger))
                using (var remoteShares = new RemoteShareCollection(remoteApplications, logger))
                using (var localApplication = new LocalApplication(config, remoteApplications, remoteShares, logger))
                using (var virtualFtpServer = new VirtualFtpServer(localApplication, logger))
                {
                    Thread.Sleep(100);
                    
                    //DoTest(virtualFtpServer);

                    Console.WriteLine("Press any key to stop...");
                    Console.ReadKey(true);
                }
            }
        }

        private static void DoTest(VirtualFtpServer virtualFtpServer)
        {
            Console.WriteLine("creating file");

            byte[] buffer = new byte[Constants.BufferSize * 2];
            var aantal = 64 * 1024;
            var size = aantal * buffer.Length;

            Stopwatch sw = Stopwatch.StartNew();
            using (var stream = virtualFtpServer.FtpHandler.FileOpenWriteCreate("/test.bin"))
            {
                for (long i = 0; i < aantal; i++)
                {
                    stream.Write(buffer, 0, buffer.Length);
                    //Thread.Sleep(200);
                }
            }
            var time = sw.Elapsed.TotalSeconds;
            var speed = size / time / 1024 / 1024;
            Console.WriteLine($"speed: {speed}mb/sec");
        }
    }
}


