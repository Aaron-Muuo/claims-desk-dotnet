using System.ComponentModel.DataAnnotations;

namespace ClaimsDesk.API.Models
{
    public class Organization
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty; // e.g., "Jubilee General Insurance"

        [Required]
        [MaxLength(50)]
        public string LicenseNumber { get; set; } = string.Empty; // Statutory regulator/IRA license code

        [Required]
        [MaxLength(50)]
        public string Slug { get; set; } = string.Empty; // URL-safe identifier (e.g., "jubilee-ke")

        [Required]
        [MaxLength(10)]
        public string CurrencyCode { get; set; } = "KES"; // Default currency for thresholds and payouts

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties for tenant isolation
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
