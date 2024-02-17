using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class InvoiceListResponse : Response
    {
        public Invoice[]? Invoices {  get; set; }
    }
}
