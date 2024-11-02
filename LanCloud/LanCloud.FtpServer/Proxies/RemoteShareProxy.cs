using LanCloud.Communication.Dtos;
using LanCloud.Servers.Wjp;

namespace LanCloud.Proxies
{
    public class RemoteShareProxy : WjpProxy
    {
        public RemoteShareProxy(RemoteShareDto config) : base(config)
        {
        }
    }
}
