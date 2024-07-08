using StorageBlob.Proxy.Interfaces;


namespace BeltmanSoftwareDesign.Data.Converters
{
    public class InvoiceConverter 
    {
        public InvoiceConverter(IStorageFileService storageFileService)
        {
        }

        public Entities.Invoice Create(Shared.Jsons.Invoice a)
        {
            //throw new NotImplementedException();
            return new Entities.Invoice()
            {
                id = a.id,
                
                Description = a.Description,
                CustomerId = a.CustomerId,
                ProjectId = a.ProjectId,
                
            };
        }

        public Shared.Jsons.Invoice Create(Entities.Invoice a)
        {
            //var test = Convert.ToInt32(1);
            //throw new NotImplementedException();
            return new Shared.Jsons.Invoice
            {
                id = a.id,
                Date = a.Date,
                IsPayed = a.IsPayedInCash,
                IsPayedInCash = a.IsPayedInCash,
                //Quarter = Convert.ToByte(Math.Ceiling(Convert.ToDouble(a.Date.Month) / 3)),
                Description = a.Description,
                CustomerId = a.CustomerId,
                CustomerName = a.Customer?.Name,
                ProjectId = a.ProjectId,
                ProjectName = a.Project?.Name,
            };
        }

        public bool Copy(Shared.Jsons.Invoice? source, Entities.Invoice dest)
        {
            //throw new NotImplementedException();
            var changed = false;
            if (dest.CustomerId != source.CustomerId)
            {
                dest.CustomerId = source.CustomerId;
                changed = true;
            }

            if (dest.Description != source.Description)
            {
                dest.Description = source.Description;
                changed = true;
            }

            if (dest.ProjectId != source.ProjectId)
            {
                dest.ProjectId = source.ProjectId;
                changed = true;
            }

            return changed;
        }
    }
}
