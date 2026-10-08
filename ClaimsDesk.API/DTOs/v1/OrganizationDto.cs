namespace ClaimsDesk.API.DTOs.v1
{
    public class OrganizationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Tin { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class CreateOrganizationDto
    {
        public string Name { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = "KES";
        public string Country { get; set; } = string.Empty;
        public string Tin { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string AccountType { get; set; } = "demo";
    }

    public class UpdateOrganizationDto
    {
        public string Name { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Tin { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
