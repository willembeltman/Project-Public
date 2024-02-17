using BeltmanSoftwareDesign.Data.Entities;
using BeltmanSoftwareDesign.Shared.Jsons;
using BeltmanSoftwareDesign.StorageBlob.Business.Interfaces;

namespace BeltmanSoftwareDesign.Data.Factories
{
    public class InvoiceFactory 
    {
        public InvoiceFactory(IStorageFileService storageFileService)
        {
        }

        public Entities.Invoice Convert(Shared.Jsons.Invoice a)
        {
            throw new NotImplementedException();
            return new Entities.Invoice()
            {
                id = a.id,
                
                Description = a.Description,
                CustomerId = a.CustomerId,
                ProjectId = a.ProjectId,
                
            };
        }

        public Shared.Jsons.Invoice Convert(Entities.Invoice a)
        {
            throw new NotImplementedException();
            return new Shared.Jsons.Invoice
            {
                id = a.id,
                Description = a.Description,
                CustomerId = a.CustomerId,
                CustomerName = a.Customer?.Name,
                ProjectId = a.ProjectId,
                ProjectName = a.Project?.Name,
            };
        }

        public bool Copy(Shared.Jsons.Invoice? source, Entities.Invoice dest)
        {
            throw new NotImplementedException();
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
