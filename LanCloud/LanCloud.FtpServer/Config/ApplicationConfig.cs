namespace LanCloud.Models
{
    public class ApplicationConfig
    {
        public string Url { get; set; }
        public int Port { get; set; }
        public string[] Folders { get; set; } = new string[0];
        public ServerConfig[] Servers { get; set; } = new ServerConfig[0];
    }
}
