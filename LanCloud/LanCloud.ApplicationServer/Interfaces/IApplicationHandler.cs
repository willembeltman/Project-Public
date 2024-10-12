namespace LanCloud.ApplicationServer.Interfaces
{
    public interface IApplicationHandler
    {
        string Receive(string request);
    }
}