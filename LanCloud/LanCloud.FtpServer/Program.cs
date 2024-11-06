using System;
using LanCloud.Services;
using LanCloud.Domain.Application;
using LanCloud.Domain.VirtualFtp;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Linq;
using LanCloud.Domain.Collections;
using LanCloud.Forms;

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

                using (var application = new LocalApplication(config, logger))
                using (var virtualFtpServer = new VirtualFtpServer(application, logger))
                {
                    Thread.Sleep(100);

                    StatusForm form = new StatusForm(application);
                    form.Show();

                    //var res = localApplication.RemoteApplications.First().Ping();

                    //DoTest(virtualFtpServer);

                    //DoTest2(virtualFtpServer);

                    Console.WriteLine("Press any key to stop...");
                    Console.ReadKey(true);
                    form.Close();
                }
            }
        }

        private static void DoTest2(VirtualFtpServer virtualFtpServer)
        {
            using (var stream = virtualFtpServer.FtpHandler.FileOpenRead("/test.bin"))
            using (var reader = new StreamReader(stream))
            {
                Console.WriteLine(reader.ReadToEnd());
                Console.WriteLine();
            }
        }

        private static void DoTest(VirtualFtpServer virtualFtpServer)
        {
            Console.WriteLine("creating file");

            byte[] buffer = new byte[Constants.BufferSize * 2];
            var aantal = 128* 1024;
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


