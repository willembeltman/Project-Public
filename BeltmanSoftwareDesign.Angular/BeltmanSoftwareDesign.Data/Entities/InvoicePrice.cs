using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class InvoicePrice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long? InvoiceId { get; set; }
        public virtual Invoice? Invoice { get; set; }

        public long? TaxRateId { get; set; }
        public virtual TaxRate? TaxRate { get; set; }

        public double Price { get; set; }
    }
}
