using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class InvoiceUpdateResponse : Response
    {
        public Invoice? Invoice {  get; set; }
    }
}
