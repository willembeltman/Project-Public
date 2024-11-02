namespace LanCloud.Configs
{
    public class Config
    {
        public int StartPort { get; set; }
        public LocalShareConfig[] Shares { get; set; } = new LocalShareConfig[0];
        public RemoteApplicationConfig[] Servers { get; set; } = new RemoteApplicationConfig[0];
        public string FileDatabaseDirectoryName { get; internal set; }
    }
}
