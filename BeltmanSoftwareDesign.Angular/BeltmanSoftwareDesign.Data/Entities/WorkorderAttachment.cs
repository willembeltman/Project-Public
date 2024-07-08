
using StorageBlob.Proxy.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class WorkorderAttachment : IStorageFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long WorkorderId { get; set; }
        public virtual Workorder? Workorder { get; set; }

        [StringLength(128)]
        public string FileMimeType { get; set; } = string.Empty;
        [StringLength(128)]
        public string FileName { get; set; } = string.Empty;
        public long FileSize { get; set; }
        [StringLength(128)]
        public string FileMD5 { get; set; } = string.Empty;


        [NotMapped]
        public string StorageFolder { get => "WorkorderAttachment"; }
        
    }
}
