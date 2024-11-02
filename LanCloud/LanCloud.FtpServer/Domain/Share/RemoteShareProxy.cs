using LanCloud.Models.Dtos;
using LanCloud.Servers.Wjp;

namespace LanCloud.Domain.Share
{
    public class RemoteShareProxy : WjpProxy
    {
        public RemoteShareProxy(RemoteShareDto config) : base(config)
        {
        }
    }
}
