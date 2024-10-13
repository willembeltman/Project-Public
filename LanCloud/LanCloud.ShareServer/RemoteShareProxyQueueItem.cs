using LanCloud.Shared.Models;
using System.Threading;

namespace LanCloud.Servers.Share
{
    public class RemoteShareProxyQueueItem
    {
        public RemoteShareProxyQueueItem(LocalShareRequest request)
        {
            Request = request;
        }
        public LocalShareRequest Request { get; set; }
        public RemoteShareResponse Response { get; set; }
        public AutoResetEvent Done { get; } = new AutoResetEvent(false);
    }
}
