using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class Workorder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long CompanyId { get; set; }
        public virtual Company? Company { get; set; }
        public long? ProjectId { get; set; }
        public virtual Project? Project { get; set; }
        public long? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        public long? RateId { get; set; }
        public virtual Rate? Rate { get; set; }

        public DateTime Start { get; set; }
        public DateTime? Stop { get; set; }
        public string? Description { get; set; }
        public double? AmountUur { get; set; }

        public virtual ICollection<InvoiceWorkorderRate>? InvoiceWorkorders { get; set; }
        public virtual ICollection<WorkorderAttachment>? WorkorderAttachments { get; set; }

    }
}
