namespace LanCloud.Configs
{
    public class ApplicationConfig
    {
        public int StartPort { get; set; }
        public string FileDatabaseDirectoryName { get; set; }
        public LocalShareConfig[] Shares { get; set; }
        public RemoteApplicationConfig[] Servers { get; set; }
    }
}
