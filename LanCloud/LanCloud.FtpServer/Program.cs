using System;
using LanCloud.Models;
using LanCloud.Services;
using LanCloud.Logger;

namespace LanCloud
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentDirectory = Environment.CurrentDirectory;
            var configService = new ConfigService(currentDirectory);
            var config = configService.Load();

            using (var localShares = new LocalShareCollection(config))
            using (var remoteApplications = new RemoteApplicationProxyCollection(config))
            using (var remoteShares = new RemoteShareCollection(remoteApplications))
            using (var localApplication = new LocalApplication(config, localShares, remoteApplications, remoteShares))
            using (var virtualFtpServer = new LocalVirtualFtp(localApplication))
            {
                Console.WriteLine("Press any key to stop...");
                Console.ReadKey(true);
            }

            LogManager.DisposeLoggers();
        }
    }
}
