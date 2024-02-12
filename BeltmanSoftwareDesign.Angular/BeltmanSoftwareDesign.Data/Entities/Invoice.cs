using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public virtual Company? Company { get; set; }
        public long CompanyId { get; set; }

        public virtual InvoiceType? InvoiceType { get; set; }
        public long? InvoiceTypeId { get; set; }
        public virtual Project? Project { get; set; }
        public long? ProjectId { get; set; }
        public virtual Customer? Customer { get; set; }
        public long? CustomerId { get; set; }
        public virtual TaxRate? TaxRate { get; set; }
        public long? TaxRateId { get; set; }

        public long? SetToPayed_By_CompanyUserId { get; set; }
        public virtual CompanyUser? SetToPayed_By_CompanyUser { get; set; }

        public string InvoiceNumber { get; set; }

        public double? RatePrice { get; set; }


        public string Description { get; set; }
        public DateTime Date { get; set; }
        public bool IsPayedInCash { get; set; }


        public bool IsPayed { get; set; }
        public DateTime? DatePayed { get; set; }
        public string PaymentCode { get; set; }

        public virtual ICollection<InvoiceWorkorderRate> InvoiceWorkorders { get; set; }
        public virtual ICollection<InvoiceRow>? InvoiceRows { get; set; }

        //public virtual ICollection<InvoiceProduct> InvoiceProducts { get; set; }
        //public virtual ICollection<InvoiceAttachment> InvoiceAttachments { get; set; }
        //public virtual ICollection<InvoiceEmail> InvoiceEmails { get; set; }
        //public virtual ICollection<Transaction> Transactions { get; set; }
        //public virtual ICollection<TransactionLog> TransactionLogs { get; set; }
        //public virtual ICollection<BankStatementInvoice> BankStatementInvoices { get; set; }
        //[NotMapped]
        //public double Tax => (RatePrice ?? 0) / 100 * (TaxRate == null ? 0 : TaxRate.Percentage);
        //[NotMapped]
        //public double ConsumentenRatePrice => (RatePrice ?? 0) + Tax;
        [NotMapped]
        public byte Quarter => Convert.ToByte(Math.Ceiling(Convert.ToDouble(Date.Month) / 3));


        //public override string ToString()
        //    => $"{Date.ToShortDateString()} {ConsumentenRatePrice.ToString("F2")} {Customer}: {Description}";
    }
}
