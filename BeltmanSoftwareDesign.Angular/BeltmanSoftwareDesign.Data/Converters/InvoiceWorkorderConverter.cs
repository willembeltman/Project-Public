namespace BeltmanSoftwareDesign.Data.Converters
{
    public class InvoiceWorkorderConverter 
    {
        public Shared.Jsons.InvoiceWorkorder Create(Entities.InvoiceWorkorder a)
        {
            return new Shared.Jsons.InvoiceWorkorder()
            {
                id = a.id,
                InvoiceId = a.InvoiceId,
                WorkorderId = a.WorkorderId,
            };
        }
    }
}
