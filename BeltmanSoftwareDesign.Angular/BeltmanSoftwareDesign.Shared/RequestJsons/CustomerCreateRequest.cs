using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.RequestJsons
{
    public class CustomerCreateRequest : Request
    {
        public Customer? Customer { get; set; }
    }
}
