using LanCloud.Models;
using LanCloud.Servers.Share;
using LanCloud.Servers.Share.Interfaces;

namespace LanCloud
{
    internal class ShareHandler : IShareHandler
    {
        public ShareHandler(ShareConfig shareConfig)
        {
            ShareConfig = shareConfig;
        }

        public ShareConfig ShareConfig { get; }

        public ShareResponse Receive(ShareRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}