using BeltmanSoftwareDesign.Data.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long CompanyId { get; set; }
        public virtual Company? Company { get; set; }
        public long? SupplierId { get; set; }
        public virtual Supplier? Supplier { get; set; }

        [Name]
        [StringLength(255)]
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ProductPrice>? ProductPrices { get; set; }
        public virtual ICollection<InvoiceProduct>? InvoiceProducts { get; set; }
        public virtual ICollection<ExpenseProduct>? ExpenseProducts { get; set; }
    }
}
