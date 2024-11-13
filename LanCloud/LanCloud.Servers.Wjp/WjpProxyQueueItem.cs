using System.Threading;

namespace LanCloud.Servers.Wjp
{
    public class WjpProxyQueueItem
    {
        public WjpProxyQueueItem(WjpRequest request)
        {
            Request = request;
        }
        public WjpRequest Request { get; set; }
        public WjpResponse Response { get; set; }
        public AutoResetEvent Done { get; } = new AutoResetEvent(false);
    }
}
