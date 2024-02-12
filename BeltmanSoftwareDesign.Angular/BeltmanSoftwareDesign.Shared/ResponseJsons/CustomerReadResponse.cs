using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class CustomerReadResponse : Response
    {
        public Customer? Customer {  get; set; }
    }
}
