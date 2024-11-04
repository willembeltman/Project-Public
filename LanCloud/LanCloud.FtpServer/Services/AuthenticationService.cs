using LanCloud.Domain.Application;
using LanCloud.Models;
using LanCloud.Shared.Log;

namespace LanCloud
{
    public class AuthenticationService
    {
        public AuthenticationService(LocalApplication localApplication, ILogger logger)
        {
            LocalApplication = localApplication;
            Logger = logger;
        }

        public LocalApplication LocalApplication { get; }
        public ILogger Logger { get; }

        public IFtpUser ValidateUser(string userName, string password)
        {
            //if (userName != "willem") return null;
            return new User(userName);
        }
    }
}