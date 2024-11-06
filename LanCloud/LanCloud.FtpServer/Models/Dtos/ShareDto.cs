using LanCloud.Servers.Wjp;

namespace LanCloud.Models.Dtos
{
    public class ShareDto : IWjpProxyConfig
    {
        public string HostName { get; set; }
        public int Port { get; set; }
    }
}