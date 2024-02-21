namespace BeltmanSoftwareDesign.Shared.Jsons
{
    public class TaxRate
    {
        public long id { get; set; }

        public long? CountryId { get; set; }
        public string? CountryName { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Percentage { get; set; }
    }
}