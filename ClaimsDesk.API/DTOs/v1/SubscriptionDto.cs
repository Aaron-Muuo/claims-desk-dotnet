using System;

namespace ClaimsDesk.API.DTOs.v1
{
    public class SubscriptionDto
    {
        public int Id { get; set; }
        public string? PlanRef { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int OrganizationId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateSubscriptionDto
    {
        public string? PlanRef { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public string Status { get; set; } = "active";
        public int OrganizationId { get; set; }
    }

    public class UpdateSubscriptionDto
    {
        public string? PlanRef { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int OrganizationId { get; set; }
    }
}
