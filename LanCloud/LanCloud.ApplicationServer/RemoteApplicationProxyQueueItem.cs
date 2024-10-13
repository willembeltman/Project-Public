using LanCloud.Servers.Application;
using LanCloud.Shared.Messages;
using System.Threading;

namespace LanCloud.Models
{
    public class RemoteApplicationProxyQueueItem
    {
        public RemoteApplicationProxyQueueItem(ApplicationMessages message)
        {
            Message = message;
        }

        public ApplicationMessages Message { get; set; }
        public string Response { get; set; }
        public AutoResetEvent Done { get; } = new AutoResetEvent(false);
    }

}