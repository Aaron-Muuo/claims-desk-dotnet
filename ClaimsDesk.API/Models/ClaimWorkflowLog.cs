using ClaimsDesk.API.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace ClaimsDesk.API.Models
{
    public class ClaimWorkflowLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClaimId { get; set; }

        [ForeignKey(nameof(ClaimId))]
        public Claim? Claim { get; set; }

        public ClaimStatus PreviousStatus { get; set; }

        public ClaimStatus NewStatus { get; set; }

        // User who triggered this state change (nullable if customer submitted/updated)
        public int? PerformedByUserId { get; set; }

        [ForeignKey(nameof(PerformedByUserId))]
        public User? PerformedByUser { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; } = string.Empty; // e.g. "Escalated: Exceeds KSh 100,000 threshold"

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
