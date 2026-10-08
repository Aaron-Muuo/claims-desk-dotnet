using ClaimsDesk.API.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimsDesk.API.Models
{
    public class Claim
    {
        [Key]
        public int Id { get; set; }

        // Multi-tenant isolation
        [Required]
        public int OrganizationId { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public Organization? Organization { get; set; }

        [Required]
        [MaxLength(50)]
        public string ClaimNumber { get; set; } = string.Empty; // e.g. "CLM-2026-0001"

        // Policyholder context (the claimant enrolled under this organization)
        [Required]
        public int MemberId { get; set; }

        [ForeignKey(nameof(MemberId))]
        public Member? Member { get; set; }

        [Required]
        [MaxLength(50)]
        public string PolicyNumber { get; set; } = string.Empty;

        // Incident details
        [Required]
        public DateTime IncidentDate { get; set; }

        public DateTime ReportedDate { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(100)]
        public string IncidentType { get; set; } = string.Empty; // e.g., "Motor Accident", "Theft", "Medical"

        [Required]
        public string Description { get; set; } = string.Empty;

        // Financials (Default Currency: KES)
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ClaimedAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ApprovedAmount { get; set; }

        [Required]
        [MaxLength(10)]
        public string CurrencyCode { get; set; } = "KES";

        // Workflow Tracking
        [Required]
        public ClaimStatus Status { get; set; } = ClaimStatus.Submitted;

        [Required]
        public SubmissionChannel Channel { get; set; } = SubmissionChannel.CustomerPortal;

        // Staff references (Nullable to support customer self-service submissions)
        public int? CreatedByUserId { get; set; } // Set if staff filed on behalf of member

        [ForeignKey(nameof(CreatedByUserId))]
        public User? CreatedByUser { get; set; }

        public int? AssignedToUserId { get; set; } // Claims Officer reviewing the claim

        [ForeignKey(nameof(AssignedToUserId))]
        public User? AssignedToUser { get; set; }

        public int? ApprovedByUserId { get; set; } // Officer/Manager/Director who authorized it

        [ForeignKey(nameof(ApprovedByUserId))]
        public User? ApprovedByUser { get; set; }

        public string? RejectionReason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<ClaimWorkflowLog> WorkflowLogs { get; set; } = new List<ClaimWorkflowLog>();
    }
}
