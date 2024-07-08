using StorageBlob.Proxy.Interfaces;

namespace BeltmanSoftwareDesign.Data.Converters
{
    public class RateConverter 
    {
        public Entities.Rate Create(Shared.Jsons.Rate source, Entities.Company currentCompany, ApplicationDbContext db)
        {
            if (source == null ||
                currentCompany == null)
                throw new NotImplementedException();

            var dest = new Entities.Rate()
            {
                id = source.id,
                Name = source.Name,
                Description = source.Description,
                Price = source.Price,
                Company = currentCompany,
                CompanyId = currentCompany.id,
            };

            Copy(source, dest, currentCompany, db);

            return dest;
        }

        public Shared.Jsons.Rate Create(Entities.Rate a)
        {
            return new Shared.Jsons.Rate
            {
                id = a.id,
                TaxRateId = a.TaxRateId,
                TaxRateName = a.TaxRate?.Name,
                Name = a.Name,
                Description = a.Description,
                Price = a.Price,
            };
        }

        public bool Copy(Shared.Jsons.Rate? source, Entities.Rate? dest, Entities.Company? currentCompany, ApplicationDbContext db)
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
            if (dest.Price != source.Price)
            {
                dest.Price = source.Price;
                changed = true;
            }

            var taxRateNameLower = (source.TaxRateName ?? "").ToLower();
            var taxRate = db.TaxRates.FirstOrDefault(a =>
                a.CompanyId == currentCompany.id &&
                a.Name.ToLower() == taxRateNameLower);

            if (taxRate == null)
            {
                taxRate = new Data.Entities.TaxRate()
                {
                    Name = source.TaxRateName,
                    CompanyId = currentCompany.id,
                    Company = currentCompany,
                };
                db.TaxRates.Add(taxRate);
                changed = true;
            }

            if (dest.TaxRateId != taxRate.id)
            {
                dest.TaxRateId = taxRate.id;
                changed = true;
            }

            return changed;
        }
    }
}
