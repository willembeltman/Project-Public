namespace BeltmanSoftwareDesign.Shared.Jsons
{
    public class Customer
    {
        public long id { get; set; }
        public long? CountryId { get; set; }
        public string? CountryName { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Postalcode { get; set; }
        public string? Place { get; set; }
        public string? PhoneNumber { get; set; }
        public string? InvoiceEmail { get; set; }

        public bool Publiekelijk { get; set; }


        //public virtual ICollection<Invoice> Invoices { get; set; }
        //public virtual ICollection<Project> Projects { get; set; }
        //public virtual ICollection<Expense> Expenses { get; set; }
        //public virtual ICollection<Workorder> Workorders { get; set; }
        //public virtual ICollection<Document> Documents { get; set; }

    }
}
