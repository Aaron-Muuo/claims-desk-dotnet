using System;

namespace ClaimsDesk.API.DTOs.v1
{
    public class MemberDto
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerNationalId { get; set; } = string.Empty;
        public string MemberNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime JoinedDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateMemberDto
    {
        public int OrganizationId { get; set; }
        public int CustomerId { get; set; }
        public string MemberNumber { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";
    }

    public class UpdateMemberDto
    {
        public string MemberNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
