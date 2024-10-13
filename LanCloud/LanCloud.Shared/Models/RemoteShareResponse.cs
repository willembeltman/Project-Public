namespace LanCloud.Shared.Models
{
    public class RemoteShareResponse
    {
        public RemoteShareResponse(string response, byte[] data)
        {
            Json = response;
            Data = data;
        }
        public string Json { get; set; }
        public byte[] Data { get; set; }
    }
}