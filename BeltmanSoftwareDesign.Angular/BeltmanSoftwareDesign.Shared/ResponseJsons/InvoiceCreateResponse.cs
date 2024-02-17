using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class InvoiceCreateResponse : Response
    {
        public Invoice? Invoice {  get; set; }
    }
}
