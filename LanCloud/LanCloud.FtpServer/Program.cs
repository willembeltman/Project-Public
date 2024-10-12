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
            var configService = new ApplicationConfigService(currentDirectory);
            var config = configService.Load();

            using (var shares = new ShareCollection(config))
            using (var externalApplications = new ApplicationProxyCollection(config))
            using (var externalShares = new ShareProxyCollection(externalApplications))
            using (var application = new Application(config, shares, externalApplications, externalShares))
            using (var virtualFtpServer = new VirtualFtp(application))
            {
                Console.WriteLine("Press any key to stop...");
                Console.ReadKey(true);
            }

            LogManager.DisposeLoggers();
        }
    }
}
