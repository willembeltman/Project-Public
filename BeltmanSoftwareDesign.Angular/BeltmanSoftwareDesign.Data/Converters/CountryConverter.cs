namespace BeltmanSoftwareDesign.Data.Converters
{
    public class CountryConverter
    {
        public Shared.Jsons.Country Create(Entities.Country a)
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
