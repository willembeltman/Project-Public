using LanCloud.Configs;
using LanCloud.Domain.Application;
using LanCloud.Models;
using LanCloud.Shared.Log;
using System;

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
            return new User(userName);
        }
    }
}