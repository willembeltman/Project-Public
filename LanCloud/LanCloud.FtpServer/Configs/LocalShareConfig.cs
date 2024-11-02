namespace LanCloud.Configs
{
    public class LocalShareConfig
    {
        public string FullName { get; set; }
        public long MaxSize { get; set; }
        public LocalSharePartConfig[] Parts { get; set; } = new LocalSharePartConfig[0];
    }
}