using LanCloud.Shared.Models;

namespace LanCloud.Shared.Interfaces
{
    public interface ILocalShareHandler
    {
        RemoteShareResponse Receive(LocalShareRequest request);
    }
}