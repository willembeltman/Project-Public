namespace LanCloud.Shared.Models
{
    public class Config
    {
        public int Port { get; set; }
        public LocalShareConfig[] Shares { get; set; } = new LocalShareConfig[0];
        public RemoteApplicationConfig[] Servers { get; set; } = new RemoteApplicationConfig[0];
    }
}
