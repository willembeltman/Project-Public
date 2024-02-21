using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.RequestJsons
{
    public class RateUpdateRequest : Request
    {
        public Rate? Rate { get; set; }
    }
}
