using BeltmanSoftwareDesign.StorageBlob.Business.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class ExperienceAttachment : IStorageFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long ExperienceId { get; set; }

        [StringLength(128)]
		public string FileMimeType { get; set; }
        [StringLength(255)]
		public string FileName { get; set; }
        public long FileSize { get; set; }
        [StringLength(128)]
        public string FileMD5 { get; set; }
        public bool Spotlight { get; set; }
        //public virtual Experience Experience { get; set; }

        [NotMapped]
        public string StorageFolder { get => "ExperienceAttachment"; }
       
    }
}
