namespace LanCloud.Shared.Models
{
    public class LocalShareRequest
    {
        public LocalShareRequest(string request, byte[] data)
        {
            Json = request;
            Data = data;
        }

        public string Json { get; set; }
        public byte[] Data { get; set; }
    }
}