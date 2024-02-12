using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.RequestJsons
{
    public class WorkorderUpdateRequest : Request
    {
        public Workorder? Workorder { get; set; }
    }
}
