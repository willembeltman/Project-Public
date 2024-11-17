namespace LanCloud.Models.Share.Requests
{
    public class CloseFileStripeSessionRequest
    {
        public CloseFileStripeSessionRequest() { }
        public CloseFileStripeSessionRequest(string path, long index)
        {
            Path = path;
            Index = index;
        }

        public string Path { get; }
        public long Index { get; }
    }
}