using System.Collections.Generic;

namespace ScrinkLargestVideos
{
    public class FFProbeRapport
    {
        public List<FFProbeStream> streams { get; set; }
        public FFProbeFormat format { get; set; }
    }
}
