namespace ClaimsDesk.API.DTOs.v1
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public bool ReceiveNotifications { get; set; }
        public string Branch { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public decimal ApprovalLimit { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateUserDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public bool ReceiveNotifications { get; set; }
        public int BranchId { get; set; }
        public int RoleId { get; set; }
        public decimal ApprovalLimit { get; set; }
    }

    public class UpdateUserDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public bool ReceiveNotifications { get; set; }
        public int BranchId { get; set; }
        public int RoleId { get; set; }
        public decimal ApprovalLimit { get; set; }
        public bool IsActive { get; set; }
    }
}
