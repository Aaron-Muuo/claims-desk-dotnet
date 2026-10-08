using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimsDesk.API.Models
{
    public class Branch
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;
        
        public bool IsHeadOffice { get; set; } = false;
        public string Address { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";
        
        [Required]
        public int OrganizationId { get; set; }
        
        [ForeignKey(nameof(OrganizationId))]
        public Organization? Organization { get; set; }
        
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
