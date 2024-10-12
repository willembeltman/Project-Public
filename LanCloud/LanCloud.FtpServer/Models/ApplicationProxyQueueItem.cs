using System.Threading;

namespace LanCloud.Models
{
    internal class ApplicationProxyQueueItem
    {
        public ApplicationProxyQueueItem(string request)
        {
            Request = request;
        }

        public string Request { get; set; }
        public string Response { get; set; }
        public AutoResetEvent RequestDone { get; } = new AutoResetEvent(false);
    }

}