using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class WorkorderListResponse : Response
    {
        public Workorder[]? Workorders {  get; set; }
    }
}
