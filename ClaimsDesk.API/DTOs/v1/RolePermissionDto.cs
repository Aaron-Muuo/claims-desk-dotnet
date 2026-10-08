namespace ClaimsDesk.API.DTOs.v1
{
    public class RolePermissionDto
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public int OrganizationId { get; set; }
    }

    public class CreateRolePermissionDto
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public int OrganizationId { get; set; }
    }
}
