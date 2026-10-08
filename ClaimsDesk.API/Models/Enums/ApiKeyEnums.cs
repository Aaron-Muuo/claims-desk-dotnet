namespace ClaimsDesk.API.Models.Enums
{
    public enum ApiKeyType
    {
        // Official ClaimsDesk clients (React Web, Mobile, Desktop)
        Platform = 1,

        // Third-party developer integrations tied to a specific insurer organization
        TenantIntegration = 2
    }

    public enum ClientPlatform
    {
        Web = 1,
        Mobile = 2,
        Desktop = 3,
        ServerToDevice = 4
    }
}
