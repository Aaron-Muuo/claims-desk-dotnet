using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimsDesk.API.Models
{
    public class AuditTrail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Code { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? SourceIp { get; set; }

        public string? UserAgent { get; set; }

        public string? RawDescription { get; set; }

        public string? RequestRoute { get; set; }

        [MaxLength(150)]
        public string? SourceController { get; set; }

        [MaxLength(150)]
        public string? SourceAction { get; set; }

        public string? Metadata { get; set; } // Stored as JSON string

        [Required]
        public int BranchId { get; set; }

        [ForeignKey(nameof(BranchId))]
        public Branch? Branch { get; set; }

        public int? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
