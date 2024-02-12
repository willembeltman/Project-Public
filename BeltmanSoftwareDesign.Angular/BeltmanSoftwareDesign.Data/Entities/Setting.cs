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
    public class Setting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }

        public string ValueString { get; set; }
        public double ValueDouble { get; set; }
        //public virtual Company Company { get; set; }
    }
}
