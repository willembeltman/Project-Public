using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.RequestJsons
{
    public class InvoiceCreateRequest : Request
    {
        public Invoice? Invoice { get; set; }
    }
}
