namespace ClaimsDesk.API.DTOs.v1
{
    public class AuditTrailDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string? SourceIp { get; set; }
        public string? UserAgent { get; set; }
        public string? RawDescription { get; set; }
        public string? RequestRoute { get; set; }
        public string? SourceController { get; set; }
        public string? SourceAction { get; set; }
        public string? Metadata { get; set; }
        public int BranchId { get; set; }
        public int? UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
