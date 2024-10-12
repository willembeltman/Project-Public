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

            using (var ownFtpShares = new FolderCollection(config))
            using (var externalApplications = new ExternalApplicationCollection(config))
            using (var externalFtpShares = new ExternalFtpCollection(externalApplications))
            using (var virtualFtpServer = new VirtualFtp(ownFtpShares, externalApplications, externalFtpShares))
            using (var application = new OwnApplication(ownFtpShares, externalApplications, externalFtpShares))
            {
                Console.WriteLine("Press any key to stop...");
                Console.ReadKey(true);
            }

            LogManager.DisposeLoggers();
        }
    }
}
