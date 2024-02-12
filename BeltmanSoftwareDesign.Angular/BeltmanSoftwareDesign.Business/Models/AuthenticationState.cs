
using BeltmanSoftwareDesign.Data.Entities;

namespace BeltmanSoftwareDesign.Business.Models
{
    public class AuthenticationState : Shared.Jsons.State
    {
        public ClientBearer? DbClientBearer { get; set; }
        public ClientDevice? DbClientDevice { get; set; }
        public ClientIpAddress? DbClientLocation { get; set; }
        public User? DbUser { get; set; }
        public Company? DbCurrentCompany { get; set; }

        //public Shared.Jsons.User? User { get; set; }
        //public Shared.Jsons.Company? CurrentCompany { get; set; }


        public bool Success { get; set; }
    }
}
