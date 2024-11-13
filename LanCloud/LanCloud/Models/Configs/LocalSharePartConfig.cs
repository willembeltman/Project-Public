using Newtonsoft.Json;
using System.Linq;

namespace LanCloud.Models.Configs
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
    }
}