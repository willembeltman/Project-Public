using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class Email
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long CompanyId { get; set; }
        public virtual Company? Company { get; set; }

        public DateTime? EmailDate { get; set; }
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public int EmailIndex { get; set; }
        public string EmailSubject { get; set; }
        public string EmailHtmlBody { get; set; }
        public string EmailTextBody { get; set; }
        public bool Hidden { get; set; }
    }
}
