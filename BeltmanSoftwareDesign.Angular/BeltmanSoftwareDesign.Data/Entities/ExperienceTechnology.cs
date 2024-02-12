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
    public class ExperienceTechnology
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long? ExperienceId { get; set; }
        public long? TechnologyId { get; set; }
        //public virtual Experience Experience { get; set; }
        //public virtual Technology Technology { get; set; }

    }
}
