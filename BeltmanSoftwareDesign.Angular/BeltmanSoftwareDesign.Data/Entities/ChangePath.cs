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

    public class ChangePath
    {
        [Key]
        public int Id { get; set; }

        public long CompanyId { get; set; }

        public string Name { get; set; }
        //public virtual ICollection<Change> Changes { get; set; } = new List<Change>();
    }
}
