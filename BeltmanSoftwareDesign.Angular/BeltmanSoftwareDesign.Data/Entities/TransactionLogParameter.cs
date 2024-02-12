using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class TransactionLogParameter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long TransactionLogId { get; set; }
        public virtual TransactionLog TransactionLog { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
