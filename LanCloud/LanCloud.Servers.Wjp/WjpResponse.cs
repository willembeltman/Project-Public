namespace LanCloud.Servers.Wjp
{
    public class WjpResponse
    {
        public WjpResponse(string response, byte[] data)
        {
            Json = response;
            Data = data;
        }
        public string Json { get; set; }
        public byte[] Data { get; set; }
    }
}