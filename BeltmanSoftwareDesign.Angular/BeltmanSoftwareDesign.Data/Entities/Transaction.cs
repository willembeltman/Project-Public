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

        public DateTime Date { get; set; }

        public double Price { get; set; }
        public double ConsumentenPrice { get; set; }
        public string TransactionId { get; set; }
        public string TransactionPaymentReference { get; set; }
        public string TransactionPaymentURL { get; set; }
        public bool? TransactionPopupAllowed { get; set; }
        public string RequestCode { get; set; }
        public string RequestMessage { get; set; }
        public bool RequestResult { get; set; }

        public string Status { get; set; }
        public bool IsPayed { get; set; }
        public DateTime? DatePayed { get; set; }
        public DateTime? BetaalAnnuleringsDate { get; set; }
        //public virtual ICollection<TransactionLog> TransactieLogs { get; set; }
        


    }
}
