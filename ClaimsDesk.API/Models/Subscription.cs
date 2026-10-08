using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimsDesk.API.Models
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        public string? PlanRef { get; set; }

        [Required]
        [MaxLength(100)]
        public string PlanName { get; set; } = string.Empty; // e.g., "growth", "professional"

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "active"; // active, inactive, canceled

        public int TopUpAiCreditsBalance { get; set; } = 0;

        [Required]
        public int OrganizationId { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public Organization? Organization { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<LicensePool> LicensePools { get; set; } = new List<LicensePool>();

        // --- Domain Helper Methods for AI Credits ---

        /// <summary>
        /// Attempts to deduct 1 AI credit using FIFO logic (earliest expiring free credits first).
        /// Requires LicensePools to be loaded.
        /// </summary>
        public bool TryDeductAiCredit()
        {
            // 1. Find the earliest expiring pool that has free credits remaining
            var earliestPool = LicensePools
                .Where(p => p.FreeAiCreditsBalance > 0 && (!p.Expires || p.ExpiresAt == null || p.ExpiresAt > DateTime.UtcNow))
                .OrderBy(p => p.ExpiresAt ?? DateTime.MaxValue)
                .FirstOrDefault();

            if (earliestPool != null)
            {
                earliestPool.FreeAiCreditsBalance -= 1;
                return true;
            }

            // 2. Fallback to top-up credits
            if (TopUpAiCreditsBalance > 0)
            {
                TopUpAiCreditsBalance -= 1;
                return true;
            }

            // 3. Out of credits
            return false;
        }

        /// <summary>
        /// Adds permanent purchased credits to the top-up balance.
        /// </summary>
        public void AddTopUpCredits(int amount)
        {
            if (amount > 0)
            {
                TopUpAiCreditsBalance += amount;
            }
        }
    }
}
