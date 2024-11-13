namespace LanCloud.Models.Configs
{
    public class LocalShareConfig
    {
        public string DirectoryName { get; set; }
        public int MaxSpeed { get; set; }
        public LocalSharePartConfig[] Parts { get; set; }
    }
}