namespace LanCloud.Servers.Wjp
{
    public class WjpRequest
    {
        public WjpRequest()
        {
        }
        public WjpRequest(int messageType, string json, byte[] data)
        {
            MessageType = messageType;
            Json = json;
            Data = data;
        }

        public int MessageType { get; set; }
        public string Json { get; set; }
        public byte[] Data { get; set; }
    }
}