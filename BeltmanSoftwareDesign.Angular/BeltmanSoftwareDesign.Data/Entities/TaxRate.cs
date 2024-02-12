using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class TaxRate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long CompanyId { get; set; }
        public virtual Company? Company { get; set; }
        public long? CountryId { get; set; }
        public virtual Country? Country { get; set; }

        public string Description { get; set; }
        public double Percentage { get; set; }

        public virtual ICollection<Invoice>? Invoices { get; set; }
        public virtual ICollection<Rate>? Rates { get; set; }

        //public virtual ICollection<ExpenseRatePrice> ExpenseRatePriceen { get; set; }
        //public virtual ICollection<WorkRate> WerkTarieven { get; set; }
        //public virtual ICollection<Product> Products { get; set; }
    }
}
