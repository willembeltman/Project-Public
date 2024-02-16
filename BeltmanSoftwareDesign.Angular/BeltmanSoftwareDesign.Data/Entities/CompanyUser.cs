//------------------------------------------------------------------------------
using BeltmanSoftwareDesign.Data.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class CompanyUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long CompanyId { get; set; }
        public virtual Company? Company { get; set; }

        [StringLength(64)]
        public string? UserId { get; set; }
        public virtual User? User { get; set; }

        //public long? CurrentProjectId { get; set; }
        ////public virtual Project CurrentProject { get; set; }
        //public long? CurrentCustomerId { get; set; }
        ////public virtual Customer CurrentCustomer { get; set; }
        //public long? CurrentSupplierId { get; set; }
        ////public virtual Supplier CurrentSupplier { get; set; }
        //public long? CurrentExpenseTypeId { get; set; }
        ////public virtual ExpenseType CurrentExpenseType { get; set; }
        //public long? CurrentInvoiceTypeId { get; set; }
        ////public virtual InvoiceType CurrentInvoiceType { get; set; }
        //public long? CurrentRateId { get; set; }
        ////public virtual Rate CurrentRate { get; set; }

        public bool Eigenaar { get; set; }
        public bool Admin { get; set; }
        public bool Actief { get; set; }

        public virtual ICollection<Invoice>? Invoices_SetToPayed { get; set; }
    }
}
