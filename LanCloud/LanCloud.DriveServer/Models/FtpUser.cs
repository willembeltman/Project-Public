using LanCloud.Ftp.Interfaces;

namespace LanCloud.Models
{
    public class FtpUser
    {
        public FtpUser(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; }
    }
}