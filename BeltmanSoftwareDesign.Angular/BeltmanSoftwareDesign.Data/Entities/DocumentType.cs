using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class DocumentType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long CompanyId { get; set; }

        public string Description { get; set; }
        //public virtual Company Company { get; set; }
        //public virtual ICollection<Document> Documents { get; set; }
    }
}
