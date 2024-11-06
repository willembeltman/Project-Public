using Newtonsoft.Json;
using System.Linq;

namespace LanCloud.Models.Configs
{
    public class LocalSharePartConfig
    {
        public LocalSharePartConfig() { }
        public LocalSharePartConfig(byte index)
        {
            Indexes = new byte[] { index };
        }
        public LocalSharePartConfig(byte[] indexes)
        {
            Indexes = indexes;
        }

        public byte[] Indexes { get; set; }
        [JsonIgnore]
        public bool IsPartity => Indexes.Length > 1;
        [JsonIgnore]
        public int? Index => IsPartity ? null as int? : Indexes.FirstOrDefault();
    }
}