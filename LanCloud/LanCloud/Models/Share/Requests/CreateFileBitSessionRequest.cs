namespace LanCloud.Models.Share.Requests
{
    public class CreateFileBitSessionRequest
    {
        public CreateFileBitSessionRequest() { }
        public CreateFileBitSessionRequest(string path)
        {
            Path = path;
        }

        public string Path { get; }
    }
}