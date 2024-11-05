using LanCloud.Models.Dtos;
using LanCloud.Servers.Wjp;

namespace LanCloud.Domain.Share
{
    public class RemoteShare : WjpProxy, IShare
    {
        public RemoteShare(RemoteShareDto config) : base(config)
        {
        }
    }
}
