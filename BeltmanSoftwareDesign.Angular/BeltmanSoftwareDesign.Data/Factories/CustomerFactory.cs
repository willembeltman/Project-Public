using BeltmanSoftwareDesign.Data.Entities;
using System.Net;

namespace BeltmanSoftwareDesign.Data.Factories
{
    public class CustomerFactory
    {
        public Shared.Jsons.Customer Convert(Customer a)
        {
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
        public Customer Convert(Shared.Jsons.Customer a)
        {
            return new Customer()
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

        public bool Copy(Shared.Jsons.Customer? source, Customer dest)
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