using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class InvoiceEmail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long InvoiceId { get; set; }        

        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime DateVerzonden { get; set; }
        public bool InvoiceGezien { get; set; }
        public DateTime? DateInvoiceGezien { get; set; }
        public bool EmailGezien { get; set; }
        public DateTime? DateEmailGezien { get; set; }
        //public virtual Invoice Invoice { get; set; }



    }
}
