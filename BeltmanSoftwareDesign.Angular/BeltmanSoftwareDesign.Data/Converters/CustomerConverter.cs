namespace BeltmanSoftwareDesign.Data.Converters
{
    public class CustomerConverter
    {
        public Shared.Jsons.Customer Create(Entities.Customer? a)
        {
            if (a == null) return null;
            return new Shared.Jsons.Customer()
            {
                id = a.id,
                Address = a.Address,
                CountryId = a.CountryId,    
                CountryName = a.Country?.Name,
                Description = a.Description,
                InvoiceEmail = a.InvoiceEmail,
                Place = a.Place,
                Postalcode = a.Postalcode,  
                PhoneNumber = a.PhoneNumber,
                Name = a.Name,
                Publiekelijk = a.Publiekelijk,
            };
        }
        public Entities.Customer Create(Shared.Jsons.Customer? a)
        {
            if (a == null) return null;
            return new Entities.Customer()
            {
                id = a.id,
                Address = a.Address,
                CountryId = a.CountryId,
                Description = a.Description,
                InvoiceEmail = a.InvoiceEmail,
                Place = a.Place,
                Postalcode = a.Postalcode,
                PhoneNumber = a.PhoneNumber,
                Name = a.Name,
                Publiekelijk = a.Publiekelijk,
            };
        }

        public bool Copy(Shared.Jsons.Customer? source, Entities.Customer dest)
        {
            var changed = false;
            if (dest.Address != source.Address) { dest.Address = source.Address; changed = true; }
            if (dest.CountryId != source.CountryId) { dest.CountryId = source.CountryId; changed = true; }
            if (dest.Description != source.Description) { dest.Description = source.Description; changed = true; }
            if (dest.InvoiceEmail != source.InvoiceEmail) { dest.InvoiceEmail = source.InvoiceEmail; changed = true; }
            if (dest.Place != source.Place) { dest.Place = source.Place; changed = true; }
            if (dest.Postalcode != source.Postalcode) { dest.Postalcode = source.Postalcode; changed = true; }
            if (dest.PhoneNumber != source.PhoneNumber) { dest.PhoneNumber = source.PhoneNumber; changed = true; }
            if (dest.Name != source.Name) { dest.Name = source.Name; changed = true; }
            if (dest.Publiekelijk != source.Publiekelijk) { dest.Publiekelijk = source.Publiekelijk; changed = true; }
            return changed;
        }
    }
}