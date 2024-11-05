namespace LanCloud.Models.Configs
{
    public class ApplicationConfig
    {
        public int StartPort { get; set; }
        public string RefDirectory { get; set; }
        public LocalShareConfig[] Shares { get; set; }
        public RemoteApplicationConfig[] Servers { get; set; }
    }
}
