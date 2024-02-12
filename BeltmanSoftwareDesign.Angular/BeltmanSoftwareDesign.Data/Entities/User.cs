using System.ComponentModel.DataAnnotations;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class User
    {
        [Key]
        [StringLength(64)]
        [Required]
        public string Id { get; set; }

        public long? CurrentCompanyId { get; set; }
        public virtual Company? CurrentCompany { get; set; }


        [StringLength(128)]
        [Required]
        public string PasswordHash { get; set; }
        public bool LockedOut { get; set; }

        [StringLength(128)]
        [Required]
        public string UserName { get; set; } = string.Empty;
        [StringLength(255)]
        [Required]
        public string Email { get; set; } = string.Empty;
        [StringLength(32)]
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        public virtual ICollection<ClientBearer> ClientBearers { get; set; } = new List<ClientBearer>();
        public virtual ICollection<CompanyUser> CompanyUsers { get; set; } = new List<CompanyUser>();
    }
    
}