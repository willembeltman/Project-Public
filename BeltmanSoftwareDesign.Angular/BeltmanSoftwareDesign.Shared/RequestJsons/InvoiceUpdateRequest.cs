using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.RequestJsons
{
    public class InvoiceUpdateRequest : Request
    {
        public Invoice? Invoice { get; set; }
    }
}
