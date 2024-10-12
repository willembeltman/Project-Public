namespace LanCloud.Servers.Share.Interfaces
{
    public interface IShareHandler
    {
        ShareResponse Receive(ShareRequest request);
    }
}