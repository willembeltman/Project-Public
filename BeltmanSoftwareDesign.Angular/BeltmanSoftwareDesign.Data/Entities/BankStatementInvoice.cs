using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class BankStatementInvoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long? BankStatementId { get; set; }
        public virtual BankStatement? BankStatement { get; set; }

        public long? InvoiceId { get; set; }
        public virtual Invoice? Invoice { get; set; }
    }
}
