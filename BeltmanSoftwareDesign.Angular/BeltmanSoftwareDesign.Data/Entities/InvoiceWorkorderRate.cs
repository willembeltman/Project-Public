using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class InvoiceWorkorderRate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long? InvoiceId { get; set; }
        public virtual Invoice? Invoice { get; set; }
        public long? WorkorderId { get; set; }
        public virtual Workorder? Workorder { get; set; }
        public long? RateId { get; set; }
        public virtual Rate? Rate { get; set; }

    }
}
