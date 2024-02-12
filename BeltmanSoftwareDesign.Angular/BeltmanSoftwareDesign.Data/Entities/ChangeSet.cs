using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class ChangeSet
    {
        [Key]
        public int Id { get; set; }

        public long CompanyId { get; set; }

        public int RealChangesetId { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(128)]
        public string Author { get; set; }
        public string Comment { get; set; }
        //public virtual ICollection<Change> Changes { get; set; } = new List<Change>();
    }
}
