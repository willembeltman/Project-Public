namespace BeltmanSoftwareDesign.Data.Factories
{
    public class InvoiceWorkorderFactory 
    {
        public Shared.Jsons.InvoiceWorkorder Convert(Entities.InvoiceWorkorder a)
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
