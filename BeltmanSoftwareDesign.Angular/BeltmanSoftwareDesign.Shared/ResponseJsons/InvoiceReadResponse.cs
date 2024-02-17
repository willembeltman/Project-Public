using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class InvoiceReadResponse : Response
    {
        public Invoice? Invoice {  get; set; }
    }
}
