using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.RequestJsons
{
    public class WorkorderCreateRequest : Request
    {
        public Workorder? Workorder { get; set; }
    }
}
