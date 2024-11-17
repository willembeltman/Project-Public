namespace LanCloud.Models.Share.Requests
{
    public class CreateFileStripeSessionRequest
    {
        public CreateFileStripeSessionRequest() { }
        public CreateFileStripeSessionRequest(string path)
        {
            Path = path;
        }

        public string Path { get; }
    }
}