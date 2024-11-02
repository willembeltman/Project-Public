namespace LanCloud.Communication.Dtos
{
    public class ShareRequestDto
    {
        public string Method { get; set; }
        public string[] Arguments { get; set; }
        public long DataSize { get; set; }
    }
}