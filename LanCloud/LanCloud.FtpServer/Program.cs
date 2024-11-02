using System;
using LanCloud.Models;
using LanCloud.Services;
using LanCloud.Shared.Log;
using System.IO;
using LanCloud.Collections;

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

                using (var localShares = new LocalShareCollection(config, logger))
                using (var remoteApplications = new RemoteApplicationProxyCollection(config, logger))
                using (var remoteShares = new RemoteShareCollection(remoteApplications, logger))
                using (var localApplication = new LocalApplication(config, localShares, remoteApplications, remoteShares, logger))
                using (var virtualFtpServer = new LocalVirtualFtp(localApplication, logger))
                {
                    Console.WriteLine("Press any key to stop...");
                    Console.ReadKey(true);
                }
            }
        }
    }
}
