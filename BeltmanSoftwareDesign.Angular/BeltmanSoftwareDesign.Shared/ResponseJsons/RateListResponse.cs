using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class RateListResponse : Response
    {
        public Rate[]? Rates {  get; set; }
    }
}
