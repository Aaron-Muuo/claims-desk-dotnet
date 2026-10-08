using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimsDesk.API.Models
{
    public class AiPromptTemplate
    {
        [Key]
        public int Id { get; set; }

        // Multi-tenant configuration (nullable if system-wide fallback default)
        public int? OrganizationId { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public Organization? Organization { get; set; }

        [Required]
        [MaxLength(50)]
        public string StageKey { get; set; } = string.Empty; // e.g., "FNOL_EXTRACTION", "FRAUD_AUDIT", "REJECTION_DRAFT"

        [Required]
        [MaxLength(100)]
        public string ModelName { get; set; } = "gpt-4o-mini";

        [Required]
        public string SystemPrompt { get; set; } = string.Empty;

        [Required]
        public string UserPromptTemplate { get; set; } = string.Empty; // Supports tokens: {{IncidentDescription}}, {{ClaimedAmount}}

        [Column(TypeName = "decimal(3,2)")]
        public decimal Temperature { get; set; } = 0.1m; // Low temperature for deterministic output

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
