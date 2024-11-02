using Newtonsoft.Json;
using System.Linq;

namespace LanCloud.Configs
{
    public class LocalSharePartConfig
    {
        public LocalSharePartConfig() { }
        public LocalSharePartConfig(int index)
        {
            Indexes = new int[] { index };
        }
        public LocalSharePartConfig(int[] indexes)
        {
            Indexes = indexes;
        }

        public int[] Indexes { get; set; }
        [JsonIgnore]
        public bool IsPartity => Indexes.Length > 1;
        [JsonIgnore]
        public int? Index => IsPartity ? null as int? : Indexes.FirstOrDefault();
    }
}