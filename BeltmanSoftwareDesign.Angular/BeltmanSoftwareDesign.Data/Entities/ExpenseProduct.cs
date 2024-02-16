using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class ExpenseProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long? ExpenseId { get; set; }
        public virtual Expense? Expense { get; set; }

        public long? ProductId { get; set; }
        public virtual Product? Product { get; set; }

        public int Amount { get; set; }
    }
}
