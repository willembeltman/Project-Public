using BeltmanSoftwareDesign.StorageBlob.Business.Interfaces;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class ExpenseAttachment : IStorageFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long? ExpenseId { get; set; }
        public string Description { get; set; }
        public string EmailUniqueId { get; set; }
        [StringLength(128)]
		public string FileMimeType { get; set; }
        [StringLength(255)]
		public string FileName { get; set; }
        public long FileSize { get; set; }
        [StringLength(128)]
        public string FileMD5 { get; set; }
        public DateTime? EmailDate { get; set; }
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public int EmailIndex { get; set; }
        public string EmailSubject { get; set; }
        public string EmailHtmlBody { get; set; }
        public string EmailTextBody { get; set; }
        public bool Hidden { get; set; }
        //public virtual Expense Expense { get; set; }

        [NotMapped]
        public string StorageFolder { get => "ExpenseAttachment"; }
    }
}
