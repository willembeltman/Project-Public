using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class WorkorderReadResponse : Response
    {
        public Workorder? Workorder {  get; set; }
    }
}
