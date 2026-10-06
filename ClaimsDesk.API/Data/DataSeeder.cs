using ClaimsDesk.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ClaimsDesk.API.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAllAsync(AppDbContext context)
        {
            await SeedRolesAsync(context);
            await SeedOrganizationsAsync(context);
            await SeedUsersAsync(context);
        }

        public static async Task SeedRolesAsync(AppDbContext context)
        {
            if (await context.Roles.AnyAsync()) return;

            var roles = new List<Role>
        {
            new() { Name = "Admin", Description = "Full system administration and user management" },
            new() { Name = "ClaimsOfficer", Description = "Intake, validation, and approvals up to KSh 100,000" },
            new() { Name = "UnderwritingManager", Description = "Escalated reviews and approvals up to KSh 500,000" },
            new() { Name = "FinanceDirector", Description = "High-exposure sign-offs above KSh 500,000" },
            new() { Name = "ComplianceAuditor", Description = "Read-only access to audit logs and statutory reports" }
        };

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }

        public static async Task SeedOrganizationsAsync(AppDbContext context)
        {
            if (await context.Organizations.AnyAsync()) return;

            var organizations = new List<Organization>
        {
            new()
            {
                Name = "Apex General Assurance",
                LicenseNumber = "IRA/042/2026",
                Slug = "apex-general",
                CurrencyCode = "KES",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "Heritage Shield Underwriters",
                LicenseNumber = "IRA/089/2026",
                Slug = "heritage-shield",
                CurrencyCode = "KES",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

            await context.Organizations.AddRangeAsync(organizations);
            await context.SaveChangesAsync();
        }

        public static async Task SeedUsersAsync(AppDbContext context)
        {
            if (await context.Users.AnyAsync()) return;

            // Fetch tenant and roles to link foreign keys
            var apexTenant = await context.Organizations.FirstOrDefaultAsync(o => o.Slug == "apex-general");
            if (apexTenant == null) return;

            var roles = await context.Roles.ToDictionaryAsync(r => r.Name, r => r.Id);

            // Default secure demo password for all initial test accounts
            string defaultHash = BCrypt.Net.BCrypt.HashPassword("ClaimsDesk@2026!");

            var users = new List<User>
        {
            // System Administrator
            new()
            {
                FullName = "Mercy Wanjiku",
                Email = "admin@apexassurance.co.ke",
                PasswordHash = defaultHash,
                OrganizationId = apexTenant.Id,
                RoleId = roles.GetValueOrDefault("Admin"),
                ApprovalLimit = 0.00m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            // Claims Officer (Tier 1: < KSh 100,000)
            new()
            {
                FullName = "Brian Omondi",
                Email = "brian.officer@apexassurance.co.ke",
                PasswordHash = defaultHash,
                OrganizationId = apexTenant.Id,
                RoleId = roles.GetValueOrDefault("ClaimsOfficer"),
                ApprovalLimit = 100000.00m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            // Underwriting Manager (Tier 2: < KSh 500,000)
            new()
            {
                FullName = "Faith Chebet",
                Email = "faith.manager@apexassurance.co.ke",
                PasswordHash = defaultHash,
                OrganizationId = apexTenant.Id,
                RoleId = roles.GetValueOrDefault("UnderwritingManager"),
                ApprovalLimit = 500000.00m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            // Finance Director (Tier 3: > KSh 500,000)
            new()
            {
                FullName = "David Mutua",
                Email = "david.director@apexassurance.co.ke",
                PasswordHash = defaultHash,
                OrganizationId = apexTenant.Id,
                RoleId = roles.GetValueOrDefault("FinanceDirector"),
                ApprovalLimit = 5000000.00m, // KSh 5M limit for high-exposure settlements
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            // Compliance & Statutory Auditor (Read-only)
            new()
            {
                FullName = "Amina Hassan",
                Email = "amina.auditor@apexassurance.co.ke",
                PasswordHash = defaultHash,
                OrganizationId = apexTenant.Id,
                RoleId = roles.GetValueOrDefault("ComplianceAuditor"),
                ApprovalLimit = 0.00m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }
    }
}
