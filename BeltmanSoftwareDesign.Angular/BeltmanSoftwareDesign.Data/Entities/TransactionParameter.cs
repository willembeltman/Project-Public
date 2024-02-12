using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class TransactionParameter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
