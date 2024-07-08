using StorageBlob.Proxy.Interfaces;

namespace BeltmanSoftwareDesign.Data.Converters
{
    public class WorkorderConverter 
    {
        public WorkorderConverter(IStorageFileService storageFileService)
        {
            InvoiceWorkorderFactory = new InvoiceWorkorderConverter();
            WorkorderAttachmentFactory = new WorkorderAttachmentConverter(storageFileService);
        }

        InvoiceWorkorderConverter InvoiceWorkorderFactory { get; }
        WorkorderAttachmentConverter WorkorderAttachmentFactory { get; }

        public Entities.Workorder Create(Shared.Jsons.Workorder source, Entities.Company currentCompany, ApplicationDbContext db)
        {
            var dest = new Entities.Workorder()
            {
                id = source.id,
                Start = source.Start,
                Stop = source.Stop,
                Name = source.Name,
                Description = source.Description,
                CustomerId = source.CustomerId,
                ProjectId = source.ProjectId,
            };

            Copy(source, dest, currentCompany, db);

            return dest;
        }

        public Shared.Jsons.Workorder Create(Entities.Workorder a)
        {
            return new Shared.Jsons.Workorder
            {
                id = a.id,
                Start = a.Start,
                Stop = a.Stop,
                Name = a.Name,
                Description = a.Description,
                CustomerId = a.CustomerId,
                CustomerName = a.Customer?.Name,
                ProjectId = a.ProjectId,
                ProjectName = a.Project?.Name,
                InvoiceWorkorders = 
                    a.InvoiceWorkorders?
                        .Select(b => InvoiceWorkorderFactory.Create(b))
                        .ToList(),
                WorkorderAttachments =
                    a.WorkorderAttachments?
                        .Select(b => WorkorderAttachmentFactory.Create(b))
                        .ToList(),
            };
        }

        public bool Copy(Shared.Jsons.Workorder? source, Entities.Workorder? dest, Entities.Company? currentCompany, ApplicationDbContext db)
        {
            if (source == null ||
                dest == null ||
                currentCompany == null)
                throw new NotImplementedException();

            if (dest.CompanyId != currentCompany.id)
                throw new Exception("Cannot change companies");

            var changed = false;

            if (dest.Name != source.Name)
            {
                dest.Description = source.Description;
                changed = true;
            }
            if (dest.Description != source.Description)
            {
                dest.Description = source.Description;
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

            dest.Customer = null;
            if (!string.IsNullOrEmpty(source.CustomerName))
            {
                dest.Customer = db.Customers.FirstOrDefault(a =>
                    a.CompanyId == currentCompany.id &&
                    a.Name.ToLower() == source.CustomerName.ToLower());
                if (dest.Customer == null)
                {
                    dest.Customer = new Data.Entities.Customer()
                    {
                        Name = source.CustomerName,
                        CompanyId = currentCompany.id,
                        Company = currentCompany,
                    };
                    db.Customers.Add(dest.Customer);
                    changed = true;
                }
            }
            dest.CustomerId = dest.Customer?.id;

            dest.Project = null;
            if (!string.IsNullOrEmpty(source.ProjectName))
            {
                dest.Project = db.Projects.FirstOrDefault(a =>
                    a.CompanyId == currentCompany.id &&
                    a.Name.ToLower() == source.ProjectName.ToLower());
                if (dest.Project == null)
                {
                    dest.Project = new Data.Entities.Project()
                    {
                        Name = source.ProjectName,
                        CompanyId = currentCompany.id,
                        Company = currentCompany,
                        Customer = dest.Customer,
                        CustomerId = dest.Customer?.id,
                    };
                    db.Projects.Add(dest.Project);
                    changed = true;
                }
            }
            dest.ProjectId = dest.Project?.id;


            return changed;
        }
    }
}
