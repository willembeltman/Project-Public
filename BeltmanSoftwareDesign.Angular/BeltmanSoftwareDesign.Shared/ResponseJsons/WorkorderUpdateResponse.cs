using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class WorkorderUpdateResponse : Response
    {
        public Workorder? Workorder {  get; set; }
    }
}
