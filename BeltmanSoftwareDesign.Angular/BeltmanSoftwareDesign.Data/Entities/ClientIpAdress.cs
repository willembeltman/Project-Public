using System.ComponentModel.DataAnnotations;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class ClientIpAddress
    {
        [Key]
        public long id { get; set; }

        [StringLength(128)]
        public string IpAddress { get; set; } = string.Empty;

        public virtual ICollection<ClientBearer> ClientBearers { get; set; } = new List<ClientBearer>();
    }
}
