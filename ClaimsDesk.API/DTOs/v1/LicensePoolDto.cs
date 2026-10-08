using System;

namespace ClaimsDesk.API.DTOs.v1
{
    public class LicensePoolDto
    {
        public int Id { get; set; }
        public string LicenseType { get; set; } = string.Empty;
        public string LicenseKey { get; set; } = string.Empty;
        public int Seats { get; set; }
        public bool Expires { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public int SubscriptionId { get; set; }
    }

    public class CreateLicensePoolDto
    {
        public string LicenseType { get; set; } = string.Empty;
        public int Seats { get; set; }
        public bool Expires { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public int SubscriptionId { get; set; }
    }

    public class UpdateLicensePoolDto
    {
        public string LicenseType { get; set; } = string.Empty;
        public int Seats { get; set; }
        public bool Expires { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public int SubscriptionId { get; set; }
    }
}
