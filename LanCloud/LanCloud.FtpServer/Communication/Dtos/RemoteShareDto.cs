using LanCloud.Servers.Wjp;

namespace LanCloud.Communication.Dtos
{
    public class RemoteShareDto : IWjpProxyConfig
    {
        public string Hostname { get; set; }
        public int Port { get; set; }
    }
}