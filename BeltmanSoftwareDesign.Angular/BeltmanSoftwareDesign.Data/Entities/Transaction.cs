using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

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
        
        public virtual ICollection<TransactionLog> TransactionLogs { get; set; }
        public virtual ICollection<TransactionParameter> TransactionParameters { get; set; }
        


    }
}
