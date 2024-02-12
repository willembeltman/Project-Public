using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class CountryListResponse : Response
    {
        public Country[]? Countries {  get; set; }
    }
}
