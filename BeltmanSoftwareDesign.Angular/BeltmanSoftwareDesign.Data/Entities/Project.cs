using BeltmanSoftwareDesign.Data.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long CompanyId { get; set; }
        public virtual Company? Company { get; set; }
        public long? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        [Name]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;
        public bool Publiekelijk { get; set; }

        public virtual ICollection<Workorder>? Workorders { get; set; }
        public virtual ICollection<Invoice>? Invoices { get; set; }
        public virtual ICollection<Expense>? Expenses { get; set; }
        public virtual ICollection<Document>? Documents { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
