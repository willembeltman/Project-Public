namespace LanCloud.Models.Share.Requests
{
    public class StoreFileStripeChunkRequest
    {
        public StoreFileStripeChunkRequest() { }
        public StoreFileStripeChunkRequest(string path, long index)
        {
            Path = path;
            Index = index;
        }

        public string Path { get; }
        public long Index { get; }
    }
}
