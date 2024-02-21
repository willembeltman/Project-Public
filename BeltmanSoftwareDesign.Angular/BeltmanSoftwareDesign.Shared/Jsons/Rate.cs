namespace BeltmanSoftwareDesign.Shared.Jsons
{
    public class Rate
    {
        public long id { get; set; }

        public long? TaxRateId { get; set; }
        public string? TaxRateName { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
    }
}