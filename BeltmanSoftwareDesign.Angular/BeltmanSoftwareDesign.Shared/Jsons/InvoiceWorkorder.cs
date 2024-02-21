namespace BeltmanSoftwareDesign.Shared.Jsons
{
    public class InvoiceWorkorder
    {
        public long id { get; set; }

        public long? InvoiceId { get; set; }
        public long? WorkorderId { get; set; }
    }
}