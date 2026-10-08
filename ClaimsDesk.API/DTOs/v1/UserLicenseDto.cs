using System;

namespace ClaimsDesk.API.DTOs.v1
{
    public class UserLicenseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LicensePoolId { get; set; }
        public DateTime? AssignedAt { get; set; }
    }

    public class CreateUserLicenseDto
    {
        public int UserId { get; set; }
        public int LicensePoolId { get; set; }
    }

    public class UpdateUserLicenseDto
    {
        public int UserId { get; set; }
        public int LicensePoolId { get; set; }
    }
}
