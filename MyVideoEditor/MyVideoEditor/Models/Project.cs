using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyVideoEditor.Models
{
    public class Project
    {
        public List<Timeline> Timelines { get; set; } = new List<Timeline>();
        public List<MediaContainer> MediaContainers { get; set; } = new List<MediaContainer>();
    }
}
