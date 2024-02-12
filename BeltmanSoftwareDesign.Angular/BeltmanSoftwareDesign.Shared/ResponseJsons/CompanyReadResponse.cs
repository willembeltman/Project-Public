using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class CompanyReadResponse : Response
    {
        public Company? Company {  get; set; }
        public bool CompanyNotFound { get; set; }
    }
}
