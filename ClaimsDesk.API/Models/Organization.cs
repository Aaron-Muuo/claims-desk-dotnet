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

        public string Country { get; set; } = string.Empty;
        public string Tin { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string AccountType { get; set; } = "demo"; // demo, live

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties for tenant isolation
        public ICollection<Branch> Branches { get; set; } = new List<Branch>();
        public ICollection<Member> Members { get; set; } = new List<Member>();
    }
}
