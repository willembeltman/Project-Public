using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class CompanyListResponse : Response
    {
        public Company[]? Companies {  get; set; }
    }
}
