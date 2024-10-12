namespace LanCloud.Servers.Application.Interfaces
{
    public interface IApplicationHandler
    {
        string Receive(string request);
    }
}