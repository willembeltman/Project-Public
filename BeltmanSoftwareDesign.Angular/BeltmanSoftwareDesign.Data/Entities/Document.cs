using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class Document
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long CompanyId { get; set; }
        public virtual Company? Company { get; set; }

        public long? DocumentTypeId { get; set; }
        public virtual DocumentType? DocumentType { get; set; }
        public long? ProjectId { get; set; }
        public virtual Project? Project { get; set; }
        public long? SupplierId { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public long? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        public DateTime Date { get; set; }
        public string Name { get; set; }

        public virtual ICollection<DocumentAttachment>? DocumentAttachments { get; set; } = new List<DocumentAttachment>();

        public byte GetQuarter()
        {
            return Convert.ToByte(Math.Ceiling(Convert.ToDouble(Date.Month) / 3));
        }

        //public override string ToString()
        //{
        //    return $"{Date.ToString("yyyy-MM-dd")} {Supplier} {Description}";
        //}
    }
}
