using Newtonsoft.Json;
using System.Linq;

namespace LanCloud.Models.Configs
{
    public class LocalShareBitConfig
    {
        public LocalShareBitConfig() { }
        public LocalShareBitConfig(int index)
        {
            Indexes = new int[] { index };
        }
        public LocalShareBitConfig(int[] indexes)
        {
            Indexes = indexes;
        }

        public int[] Indexes { get; set; }
    }
}