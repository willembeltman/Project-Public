using LanCloud.Servers.Wjp;

namespace LanCloud.Models.Dtos
{
    public class RemoteShareDto : IWjpProxyConfig
    {
        public string Hostname { get; set; }
        public int Port { get; set; }
    }
}