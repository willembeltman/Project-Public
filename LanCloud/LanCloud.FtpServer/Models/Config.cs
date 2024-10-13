using LanCloud.Shared.Models;

namespace LanCloud.Models
{
    public class Config
    {
        public int Port { get; set; }
        public LocalShareConfig[] Shares { get; set; } = new LocalShareConfig[0];
        public ServerConfig[] Servers { get; set; } = new ServerConfig[0];
    }
}
