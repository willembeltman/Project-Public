using BeltmanSoftwareDesign.Data.Attributes;
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
        public long CountryId { get; set; }
        public virtual Country? Country { get; set; }

        [Name]
        [StringLength(255)]
        public string Name { get; set; }
        public string Description { get; set; }
        public double Percentage { get; set; }

        public virtual ICollection<Rate>? Rates { get; set; }
        public virtual ICollection<ExpensePrice>? ExpensePrices { get; set; }
        public virtual ICollection<ProductPrice>? ProductPrices { get; set; }
        public virtual ICollection<InvoicePrice>? InvoicePrices { get; set; }
        public virtual ICollection<InvoiceRow>? InvoiceRows { get; set; }
    }
}
