using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimsDesk.API.Models
{
    public class ClaimAiAnalysis
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClaimId { get; set; }

        [ForeignKey(nameof(ClaimId))]
        public Claim? Claim { get; set; }

        public int? PromptTemplateId { get; set; }

        [ForeignKey(nameof(PromptTemplateId))]
        public AiPromptTemplate? PromptTemplate { get; set; }

        [Required]
        [MaxLength(50)]
        public string AnalysisType { get; set; } = string.Empty; // "DocumentExtraction", "FraudAssessment", "Summary"

        // Risk Evaluation
        public int RiskScore { get; set; } = 0; // 0 (Clean) to 100 (Severe Fraud Risk)

        [MaxLength(50)]
        public string RecommendedAction { get; set; } = "Proceed"; // "Proceed", "FlagForInvestigation", "Reject"

        [Column(TypeName = "decimal(18,2)")]
        public decimal? RecommendedReserveKSh { get; set; }

        public string Summary { get; set; } = string.Empty;

        // Structured JSON result returned by the LLM
        public string ExtractedDataJson { get; set; } = "{}";

        public string FlaggedAnomaliesJson { get; set; } = "[]";

        // Telemetry & Audit
        public int PromptTokens { get; set; }
        public int CompletionTokens { get; set; }
        public int ExecutionDurationMs { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
