namespace ClaimsDesk.API.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Organization { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public decimal ApprovalLimit { get; set; }
        public bool IsActive { get; set; }
    }
}
