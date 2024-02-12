using BeltmanSoftwareDesign.Data.Entities;

namespace BeltmanSoftwareDesign.Data.Factories
{
    public class CountryFactory
    {
        public Shared.Jsons.Country Convert(Country a)
        {
            return new Shared.Jsons.Country()
            {
                id = a.id,
                Code = a.Code,
                Name = a.Name
            };
        }
    }
}
