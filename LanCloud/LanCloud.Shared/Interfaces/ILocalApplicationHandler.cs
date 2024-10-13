using LanCloud.Shared.Messages;

namespace LanCloud.Shared.Interfaces
{
    public interface ILocalApplicationHandler
    {
        string Receive(ApplicationMessages message);
    }
}