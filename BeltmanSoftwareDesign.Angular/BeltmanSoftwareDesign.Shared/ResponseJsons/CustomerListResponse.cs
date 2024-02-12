using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class CustomerListResponse : Response
    {
        public Customer[]? Customers {  get; set; }
    }
}
