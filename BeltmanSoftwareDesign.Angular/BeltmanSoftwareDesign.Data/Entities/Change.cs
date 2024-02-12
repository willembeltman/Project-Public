using System.ComponentModel.DataAnnotations;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class Change
    {
        [Key]
        public int Id { get; set; }

        public int? ChangesetId { get; set; }
        public int? ChangeSetPathId { get; set; }

        [StringLength(64)]
        public string ChangeType { get; set; }
        public long Size { get; set; }
        //public virtual ChangeSet ChangeSet { get; set; }
        //public virtual ChangePath ChangePath { get; set; }
    }
}
