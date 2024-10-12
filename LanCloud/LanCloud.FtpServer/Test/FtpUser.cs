using LanCloud.FtpServer.Interfaces;

namespace LanCloud.Application
{
    internal class FtpUser : IFtpUser
    {
        public FtpUser(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; }
    }
}