using System.ComponentModel.DataAnnotations;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class ClientDevice
    {
        [Key]
        public long id { get; set; }

        [StringLength(128)]
        public string DeviceHash { get; set; } = string.Empty;

        public virtual ICollection<ClientBearer>? ClientBearers { get; set; } = new List<ClientBearer>();
        public virtual ICollection<ClientDeviceProperty>? ClientDeviceProperties {  get; set; } = new List<ClientDeviceProperty>();
    }
}
