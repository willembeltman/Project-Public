using LanCloud.Models;

namespace LanCloud.Domain.Application
{
    public class User : IFtpUser
    {
        public User(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; set; }
    }
}
