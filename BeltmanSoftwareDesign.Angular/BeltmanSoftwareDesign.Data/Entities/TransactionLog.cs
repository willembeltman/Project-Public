using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class TransactionLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long? TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual ICollection<TransactionLogParameter> TransactionLogParameters { get; set; }

    }
}
