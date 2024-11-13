namespace LanCloud.Servers.Wjp
{
    public interface IWjpHandler
    {
        WjpResponse ProcessRequest(WjpRequest request);
    }
}