namespace BeltmanSoftwareDesign.Shared.Jsons
{
    public class Project
    {
        public long id { get; set; }

        public long? CustomerId { get; set; }
        public string? CustomerName { get; set; }

        public string? Name { get; set; }
        public bool Publiekelijk { get; set; }
    }
}