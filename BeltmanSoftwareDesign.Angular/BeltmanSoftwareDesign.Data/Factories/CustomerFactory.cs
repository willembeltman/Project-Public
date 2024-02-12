using BeltmanSoftwareDesign.Data.Entities;

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
    }
}