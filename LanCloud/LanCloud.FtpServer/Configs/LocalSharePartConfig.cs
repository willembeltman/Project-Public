using System.Linq;

namespace LanCloud.Configs
{
    public class LocalSharePartConfig
    {
        public LocalSharePartConfig() { }
        public LocalSharePartConfig(int index)
        {
            IsPartity = false;
            Indexes = new int[] { index };
        }
        public LocalSharePartConfig(int[] indexes)
        {
            IsPartity = true;
            Indexes = indexes;
        }

        public bool IsPartity { get; set; }
        public int[] Indexes { get; set; }
        public int? Index => IsPartity ? null as int? : Indexes.FirstOrDefault();
    }
}