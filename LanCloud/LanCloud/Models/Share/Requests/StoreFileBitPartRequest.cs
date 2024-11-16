namespace LanCloud.Models.Share.Requests
{
    public class StoreFileBitPartRequest
    {
        public StoreFileBitPartRequest() { }
        public StoreFileBitPartRequest(string path, long index)
        {
            Path = path;
            Index = index;
        }

        public string Path { get; }
        public long Index { get; }
    }
}
