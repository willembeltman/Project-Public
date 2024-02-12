using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long CompanyId { get; set; }
        public long? TaxRateId { get; set; }
        public long? SupplierId { get; set; }

        public string Description { get; set; }
        public double Price { get; set; }
        //public virtual Company Company { get; set; }
        //public virtual TaxRate TaxRate { get; set; }
        //public virtual Supplier Supplier { get; set; }
        //public virtual ICollection<InvoiceProduct> InvoiceProducts { get; set; }
    }
}
