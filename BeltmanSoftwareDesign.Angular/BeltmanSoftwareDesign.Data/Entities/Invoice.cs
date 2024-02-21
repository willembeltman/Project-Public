using BeltmanSoftwareDesign.Data.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long CompanyId { get; set; }
        public virtual Company? Company { get; set; }
        public long? InvoiceTypeId { get; set; }
        public virtual InvoiceType? InvoiceType { get; set; }
        public long? ProjectId { get; set; }
        public virtual Project? Project { get; set; }
        public long? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        public long? IsPayedInCash_By_CompanyUserId { get; set; }
        public virtual CompanyUser? IsPayedInCash_By_CompanyUser { get; set; }

        [Name]
        [StringLength(255)]
        public string InvoiceNumber { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public bool IsPayedInCash { get; set; }

        public virtual ICollection<InvoiceRow>? InvoiceRows { get; set; } = new List<InvoiceRow>();
        public virtual ICollection<InvoiceWorkorder> InvoiceWorkorders { get; set; } = new List<InvoiceWorkorder>();
        public virtual ICollection<InvoiceProduct>? InvoiceProducts { get; set; } = new List<InvoiceProduct>();
        public virtual ICollection<InvoicePrice>? InvoicePrices { get; set; } = new List<InvoicePrice>();
        public virtual ICollection<InvoiceAttachment>? InvoiceAttachments { get; set; } = new List<InvoiceAttachment>();
        public virtual ICollection<InvoiceEmail>? InvoiceEmails { get; set; } = new List<InvoiceEmail>();
        public virtual ICollection<InvoiceTransaction>? InvoiceTransactions { get; set; } = new List<InvoiceTransaction>();
        public virtual ICollection<BankStatementInvoice>? BankStatementInvoices { get; set; } = new List<BankStatementInvoice>();


        //[NotMapped]
        //public double Tax => (RatePrice ?? 0) / 100 * (TaxRate == null ? 0 : TaxRate.Percentage);
        //[NotMapped]
        //public double ConsumentenRatePrice => (RatePrice ?? 0) + Tax;
        //[NotMapped]
        //public byte Quarter => Convert.ToByte(Math.Ceiling(Convert.ToDouble(Date.Month) / 3));


        //public override string ToString()
        //    => $"{Date.ToShortDateString()} {ConsumentenRatePrice.ToString("F2")} {Customer}: {Description}";
    }
}
