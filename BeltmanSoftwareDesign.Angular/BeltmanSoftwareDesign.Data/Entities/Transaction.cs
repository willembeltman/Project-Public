using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long CompanyId { get; set; }
        public virtual Company? Company { get; set; }

        public DateTime Date { get; set; }
        public double Price { get; set; }

        public string ExternalTransactionId { get; set; }

        public bool IsPayed { get; set; }
        public DateTime? DatePayed { get; set; }
        public DateTime? DateCancelled { get; set; }
        
        public virtual ICollection<InvoiceTransaction>? InvoiceTransactions { get; set; } = new List<InvoiceTransaction>();
        public virtual ICollection<TransactionLog>? TransactionLogs { get; set; } = new List<TransactionLog>();
        public virtual ICollection<TransactionParameter>? TransactionParameters { get; set; } = new List<TransactionParameter>();  
    }
}
