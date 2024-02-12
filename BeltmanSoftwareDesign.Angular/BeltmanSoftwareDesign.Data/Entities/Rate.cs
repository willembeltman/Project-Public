using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltmanSoftwareDesign.Data.Entities
{

    public class Rate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long CompanyId { get; set; }
        public virtual Company? Company { get; set; }
        public long? TaxRateId { get; set; }
        public virtual TaxRate? TaxRate { get; set; }

        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public double? Tax { get; set; }
        public virtual ICollection<InvoiceWorkorder>? InvoiceWorkorders { get; set; }
        public virtual ICollection<Workorder>? Workorders { get; set; }

        //public double GetTax()
        //{
        //    if (Tax == null)
        //    {
        //        Tax = Price / 100 * (TaxRate == null ? 0 : TaxRate.Percentage);
        //    }
        //    return Tax.Value;
        //}
    }
}
