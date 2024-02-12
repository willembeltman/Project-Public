using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.RequestJsons
{
    public class CompanyUpdateRequest : Request
    {
        public Company? Company { get; set; }
    }
}
