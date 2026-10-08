using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimsDesk.API.Models
{
    public class LicensePool
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string LicenseType { get; set; } = string.Empty; // demo, developer, enterprise

        [Required]
        public string LicenseKey { get; set; } = string.Empty;

        
        public int Seats { get; set; } = 0;

        public int FreeAiCreditsBalance { get; set; } = 0;

        public bool Expires { get; set; } = true;

        public DateTime? ExpiresAt { get; set; }

        [Required]
        public int SubscriptionId { get; set; }

        [ForeignKey(nameof(SubscriptionId))]
        public Subscription? Subscription { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<UserLicense> UserLicenses { get; set; } = new List<UserLicense>();
    }
}
