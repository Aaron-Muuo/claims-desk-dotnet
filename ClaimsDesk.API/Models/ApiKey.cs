using ClaimsDesk.API.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimsDesk.API.Models
{
    public class ApiKey
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string AppName { get; set; } = string.Empty; // e.g., "ClaimsDesk Web Portal", "Apex Core Banking Sync"

        // Public identifier passed in headers (e.g., "cd_app_9a8b7c6d")
        [Required]
        [MaxLength(64)]
        public string AppId { get; set; } = string.Empty;

        // Cryptographic hash (SHA-256) of the secret. Never store raw keys.
        [Required]
        [MaxLength(128)]
        public string KeyHash { get; set; } = string.Empty;

        // Truncated preview for display in admin panels (e.g., "cd_sec_9a8b...3f1a")
        [Required]
        [MaxLength(20)]
        public string KeyPrefix { get; set; } = string.Empty;

        [Required]
        public ApiKeyType Type { get; set; } = ApiKeyType.TenantIntegration;

        [Required]
        public ClientPlatform Platform { get; set; } = ClientPlatform.Web;

        // Multi-tenant isolation:
        // Null for Platform clients (React Web/Mobile); Required for Tenant integrations.
        public int? OrganizationId { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public Organization? Organization { get; set; }

        // Space- or comma-separated permissions (e.g. "claims:read claims:submit")
        [MaxLength(500)]
        public string AllowedScopes { get; set; } = "*";

        // Rate limiting per application (requests per minute)
        public int RateLimitPerMinute { get; set; } = 60;

        public bool IsActive { get; set; } = true;

        public DateTime? ExpiresAt { get; set; }

        public DateTime? LastUsedAt { get; set; }

        // Internal staff member who issued the key
        public int? CreatedByUserId { get; set; }

        [ForeignKey(nameof(CreatedByUserId))]
        public User? CreatedByUser { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedAt { get; set; }
    }
}
