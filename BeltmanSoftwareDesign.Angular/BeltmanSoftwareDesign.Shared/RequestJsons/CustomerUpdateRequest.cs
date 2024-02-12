using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.RequestJsons
{
    public class CustomerUpdateRequest : Request
    {
        public Customer? Customer { get; set; }
    }
}
