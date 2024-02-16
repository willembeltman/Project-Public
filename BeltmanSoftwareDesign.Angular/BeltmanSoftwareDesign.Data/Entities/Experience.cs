using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltmanSoftwareDesign.Data.Entities
{

    public class Experience
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long CompanyId { get; set; }
        //public virtual Company? Company { get; set; }
        public long? ProjectId { get; set; }
        //public virtual Project? Project { get; set; }
        public long? CustomerId { get; set; }
        //public virtual Customer? Customer { get; set; }

        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(1024)]
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime? Stop { get; set; }
        public int? AmountUur { get; set; }
        
        //public virtual ICollection<ExperienceAttachment>? ExperienceAttachments { get; set; }
        //public virtual ICollection<ExperienceTechnology>? ExperienceTechnologyen { get; set; }
        
    }
}
