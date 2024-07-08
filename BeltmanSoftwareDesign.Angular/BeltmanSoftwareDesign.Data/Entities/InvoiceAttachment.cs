using StorageBlob.Proxy.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class InvoiceAttachment : IStorageFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long InvoiceId { get; set; }
        public virtual Invoice? Invoice { get; set; }

        public DateTime? Date { get; set; }
        public bool IsInvoicePDF { get; set; }
        public bool IsWorkorderPDF { get; set; }


        [StringLength(128)]
		public string FileMimeType { get; set; }
        [StringLength(255)]
		public string FileName { get; set; }
        public long FileSize { get; set; }
        [StringLength(128)]
        public string FileMD5 { get; set; }

        [NotMapped]
        public string StorageFolder { get => "InvoiceAttachment"; }

    }
}
