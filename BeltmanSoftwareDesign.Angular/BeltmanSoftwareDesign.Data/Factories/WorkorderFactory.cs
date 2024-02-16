using BeltmanSoftwareDesign.Data.Entities;
using BeltmanSoftwareDesign.Shared.Jsons;
using BeltmanSoftwareDesign.StorageBlob.Business.Interfaces;

namespace BeltmanSoftwareDesign.Data.Factories
{
    public class WorkorderFactory 
    {
        public WorkorderFactory(IStorageFileService storageFileService)
        {
            InvoiceWorkorderFactory = new InvoiceWorkorderFactory();
            WorkorderAttachmentFactory = new WorkorderAttachmentFactory(storageFileService);
        }

        InvoiceWorkorderFactory InvoiceWorkorderFactory { get; }
        WorkorderAttachmentFactory WorkorderAttachmentFactory { get; }

        public Entities.Workorder Convert(Shared.Jsons.Workorder a)
        {
            return new Entities.Workorder()
            {
                id = a.id,
                CompanyId = a.CompanyId,
                Start = a.Start,
                Stop = a.Stop,
                Description = a.Description,
                CustomerId = a.CustomerId,
                ProjectId = a.ProjectId,
            };
        }

        public Shared.Jsons.Workorder Convert(Entities.Workorder a)
        {
            return new Shared.Jsons.Workorder
            {
                id = a.id,
                CompanyId = a.CompanyId,
                Start = a.Start,
                Stop = a.Stop,
                Description = a.Description,
                CustomerId = a.CustomerId,
                CustomerName = a.Customer.Name,
                ProjectId = a.ProjectId,
                ProjectName = a.Project.Name,
                InvoiceWorkorders = a.InvoiceWorkorders
                    .Select(b => InvoiceWorkorderFactory.Convert(b))
                    .ToList(),
                WorkorderAttachments =
                    a.WorkorderAttachments
                    .Select(b => WorkorderAttachmentFactory.Convert(b))
                    .ToList(),
            };
        }

        public bool Copy(Shared.Jsons.Workorder? source, Entities.Workorder dest)
        {
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

            if (dest.Start != source.Start)
            {
                dest.Start = source.Start;
                changed = true;
            }

            if (dest.Stop != source.Stop)
            {
                dest.Stop = source.Stop;
                changed = true;
            }
            return changed;
        }
    }
}
