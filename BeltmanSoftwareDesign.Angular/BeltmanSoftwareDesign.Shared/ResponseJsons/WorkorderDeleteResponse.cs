using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class WorkorderDeleteResponse : Response
    {
        public bool ErrorWorkorderNotFound { get; set; }
        public bool ErrorCurrentCompanyDifferentThanWorkorderCompany { get; set; }
    }
}
