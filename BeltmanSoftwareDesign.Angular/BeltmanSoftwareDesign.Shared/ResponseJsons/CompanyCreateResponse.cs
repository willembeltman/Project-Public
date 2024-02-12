using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class CompanyCreateResponse : Response
    {
        public Company? Company {  get; set; }
        public bool ErrorCompanyNameAlreadyUsed { get; set; }
    }
}
