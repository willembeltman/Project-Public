namespace LanCloud.Models
{
    public class ApplicationConfig
    {
        public int Port { get; set; }
        public ShareConfig[] Shares { get; set; } = new ShareConfig[0];
        public ServerConfig[] Servers { get; set; } = new ServerConfig[0];
    }
}
