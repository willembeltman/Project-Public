using BeltmanSoftwareDesign.Data.Entities;
using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Data.Factories
{
    public class InvoiceWorkorderFactory 
    {
        public Shared.Jsons.InvoiceWorkorder Convert(Entities.InvoiceWorkorderRate a)
        {
            return new Shared.Jsons.InvoiceWorkorder()
            {
                id = a.id,
                InvoiceId = a.InvoiceId,
                RateId = a.RateId,
                WorkorderId = a.WorkorderId,
            };
        }
    }
}
