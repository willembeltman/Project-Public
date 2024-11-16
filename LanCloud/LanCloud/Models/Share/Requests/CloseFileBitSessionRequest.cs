namespace LanCloud.Models.Share.Requests
{
    public class CloseFileBitSessionRequest
    {
        public CloseFileBitSessionRequest() { }
        public CloseFileBitSessionRequest(string path, long index)
        {
            Path = path;
            Index = index;
        }

        public string Path { get; }
        public long Index { get; }
    }
}