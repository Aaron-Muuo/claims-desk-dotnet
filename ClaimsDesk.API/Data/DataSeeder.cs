using ClaimsDesk.API.Models;
using Microsoft.EntityFrameworkCore;

using ClaimsDesk.API.Helpers;

namespace ClaimsDesk.API.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAllAsync(AppDbContext context, ILicenseKeyService licenseKeyService)
        {
            await SeedRolesAsync(context);
            await SeedOrganizationsAsync(context);
            await SeedPermissionsAsync(context);
            await SeedRolePermissionsAsync(context);
            await SeedBranchesAsync(context);
            await SeedUsersAsync(context);
            await SeedSubscriptionsAsync(context, licenseKeyService);
            await SeedCustomersAndMembersAsync(context);
        }

        public static async Task SeedRolesAsync(AppDbContext context)
        {
            if (await context.Roles.AnyAsync()) return;

            var roles = new List<Role>
        {
            new() { Name = "SuperAdmin", Description = "Full unrestricted access across the entire organization" },
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
                Country = "Kenya",
                Tin = "P001234567A",
                Address = "Nairobi, Kenya",
                AccountType = "live",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "Heritage Shield Underwriters",
                LicenseNumber = "IRA/089/2026",
                Slug = "heritage-shield",
                CurrencyCode = "KES",
                Country = "Kenya",
                Tin = "P009876543B",
                Address = "Mombasa, Kenya",
                AccountType = "demo",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

            await context.Organizations.AddRangeAsync(organizations);
            await context.SaveChangesAsync();
        }

        public static async Task SeedPermissionsAsync(AppDbContext context)
        {
            if (await context.Permissions.AnyAsync()) return;

            var permissions = new List<Permission>
            {
                new() { Name = "can_view_claims", Description = "View claim files, documentation, timeline events, and loss adjustor notes." },
                new() { Name = "can_create_claims", Description = "Register First Notice of Loss (FNOL) and file claims on behalf of policyholders." },
                new() { Name = "can_edit_claim_details", Description = "Modify claim metadata, loss dates, incident descriptions, and categorization." },
                new() { Name = "can_adjust_claim_reserves", Description = "Set, increase, or decrease initial reserve funds allocated for claim liability." },
                new() { Name = "can_reassign_claims", Description = "Reassign claim files to different officers, assessors, or external adjustors." },
                new() { Name = "can_reject_claims", Description = "Formally repudiate a claim with statutory justification notes and policy exclusion codes." },
                new() { Name = "can_reopen_claims", Description = "Reopen settled or repudiated claims following arbitration, disputes, or supplemental bills." },
                new() { Name = "can_approve_claim", Description = "Approve claim amounts for settlement." },
                new() { Name = "can_authorize_payouts", Description = "Authorize financial payouts for approved claims." },
                
                new() { Name = "can_view_members", Description = "View member directories, KYC documentation, National ID/passport details, and history." },
                new() { Name = "can_create_members", Description = "Enroll new policyholders and assign member identification numbers to the organization." },
                new() { Name = "can_edit_members", Description = "Update contact information, address records, and beneficiary structures." },
                new() { Name = "can_suspend_members", Description = "Place a hold on member benefits or mark membership status as lapsed/suspended." },
                
                new() { Name = "can_view_policies", Description = "View policy schedules, sum insured limits, deductibles/excesses, and active endorsements." },
                new() { Name = "can_create_policies", Description = "Issue new policy contracts and bind coverages under the tenant organization." },
                new() { Name = "can_edit_policies", Description = "Update policy conditions, rider attachments, premium terms, or expiration dates." },
                new() { Name = "can_cancel_policies", Description = "Terminate coverage contracts before expiration due to non-payment or fraud." },
                
                new() { Name = "can_view_users", Description = "View internal staff profiles, assigned operational roles, and current statuses." },
                new() { Name = "can_create_users", Description = "Provision internal staff accounts and issue invitations." },
                new() { Name = "can_edit_users", Description = "Modify staff profiles, reassign operational roles, or change financial approval limits." },
                new() { Name = "can_deactivate_users", Description = "Revoke staff authentication access and deactivate credentials." },
                new() { Name = "can_manage_roles", Description = "Create custom tenant roles and adjust the granular permission mapping table." },
                
                new() { Name = "can_view_settings", Description = "View tenant configurations, regulatory license details, and operating currencies." },
                new() { Name = "can_edit_settings", Description = "Update organization profile data, contact addresses, and operational configurations." },
                
                new() { Name = "can_view_audit_logs", Description = "Inspect immutable audit trails, credential events, data updates, and authentication logs." },
                
                new() { Name = "can_configure_sla", Description = "Set FNOL intake deadlines, assessment SLA timers, and supervisor escalation triggers." }
            };

            await context.Permissions.AddRangeAsync(permissions);
            await context.SaveChangesAsync();
        }

        public static async Task SeedRolePermissionsAsync(AppDbContext context)
        {
            if (await context.RolePermissions.AnyAsync()) return;

            var org = await context.Organizations.FirstOrDefaultAsync();
            if (org == null) return;

            var permissions = await context.Permissions.ToDictionaryAsync(p => p.Name, p => p.Id);
            var roles = await context.Roles.ToDictionaryAsync(r => r.Name, r => r.Id);

            var rolePermissionMaps = new Dictionary<string, List<string>>
            {
                { "SuperAdmin", permissions.Keys.ToList() },
                { "Admin", new List<string> { 
                    "can_view_claims", "can_create_claims", "can_reassign_claims", "can_approve_claim", "can_authorize_payouts",
                    "can_view_members", "can_create_members", "can_edit_members", "can_suspend_members",
                    "can_view_policies", "can_create_policies", "can_edit_policies", 
                    "can_view_users", "can_create_users", "can_edit_users", "can_deactivate_users", "can_manage_roles",
                    "can_view_settings", "can_edit_settings", "can_view_audit_logs", "can_configure_sla" 
                }},
                { "ClaimsOfficer", new List<string> {
                    "can_view_claims", "can_create_claims", "can_edit_claim_details", "can_adjust_claim_reserves", "can_approve_claim",
                    "can_view_members", "can_create_members", "can_edit_members",
                    "can_view_policies"
                }},
                { "UnderwritingManager", new List<string> {
                    "can_view_claims", "can_edit_claim_details", "can_adjust_claim_reserves", "can_reassign_claims", "can_reject_claims", "can_reopen_claims", "can_approve_claim", "can_authorize_payouts",
                    "can_view_members", "can_suspend_members",
                    "can_view_policies", "can_create_policies", "can_edit_policies", "can_cancel_policies",
                    "can_view_users",
                    "can_configure_sla"
                }},
                { "FinanceDirector", new List<string> {
                    "can_view_claims", "can_reject_claims", "can_reopen_claims", "can_approve_claim", "can_authorize_payouts",
                    "can_view_policies", "can_cancel_policies"
                }},
                { "ComplianceAuditor", new List<string> {
                    "can_view_claims", "can_view_members", "can_view_policies", "can_view_settings", "can_view_audit_logs"
                }}
            };

            var rolePermissions = new List<RolePermission>();

            foreach (var map in rolePermissionMaps)
            {
                var roleName = map.Key;
                var rolePerms = map.Value;

                if (roles.TryGetValue(roleName, out int roleId))
                {
                    foreach (var permName in rolePerms)
                    {
                        if (permissions.TryGetValue(permName, out int permId))
                        {
                            rolePermissions.Add(new RolePermission
                            {
                                RoleId = roleId,
                                PermissionId = permId,
                                OrganizationId = org.Id
                            });
                        }
                    }
                }
            }

            await context.RolePermissions.AddRangeAsync(rolePermissions);
            await context.SaveChangesAsync();
        }

        public static async Task SeedBranchesAsync(AppDbContext context)
        {
            if (await context.Branches.AnyAsync()) return;

            var apexTenant = await context.Organizations.FirstOrDefaultAsync(o => o.Slug == "apex-general");
            if (apexTenant == null) return;

            var branches = new List<Branch>
            {
                new()
                {
                    Name = "Nairobi HQ",
                    Code = "HQ-001",
                    IsHeadOffice = true,
                    Address = "Upper Hill, Nairobi",
                    Status = "Active",
                    OrganizationId = apexTenant.Id
                },
                new()
                {
                    Name = "Mombasa Branch",
                    Code = "MBS-002",
                    IsHeadOffice = false,
                    Address = "Moi Avenue, Mombasa",
                    Status = "Active",
                    OrganizationId = apexTenant.Id
                }
            };

            await context.Branches.AddRangeAsync(branches);
            await context.SaveChangesAsync();
        }

        public static async Task SeedUsersAsync(AppDbContext context)
        {
            if (await context.Users.AnyAsync()) return;

            // Fetch branch and roles to link foreign keys
            var mainBranch = await context.Branches.FirstOrDefaultAsync(b => b.Code == "HQ-001");
            if (mainBranch == null) return;

            var roles = await context.Roles.ToDictionaryAsync(r => r.Name, r => r.Id);

            // Default secure demo password for all initial test accounts
            string defaultHash = BCrypt.Net.BCrypt.HashPassword("ClaimsDesk@2026!");

            var users = new List<User>
        {
            // Super Administrator
            new()
            {
                FullName = "John Doe",
                Username = "john.doe",
                PhoneNumber = "+254700000000",
                Gender = "male",
                ReceiveNotifications = true,
                Email = "superadmin@apexassurance.co.ke",
                PasswordHash = defaultHash,
                BranchId = mainBranch.Id,
                RoleId = roles.GetValueOrDefault("SuperAdmin"),
                ApprovalLimit = 0.00m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            // System Administrator
            new()
            {
                FullName = "Mercy Wanjiku",
                Username = "mercy.wanjiku",
                PhoneNumber = "+254700000001",
                Gender = "female",
                ReceiveNotifications = true,
                Email = "admin@apexassurance.co.ke",
                PasswordHash = defaultHash,
                BranchId = mainBranch.Id,
                RoleId = roles.GetValueOrDefault("Admin"),
                ApprovalLimit = 0.00m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            // Claims Officer (Tier 1: < KSh 100,000)
            new()
            {
                FullName = "Brian Omondi",
                Username = "brian.omondi",
                PhoneNumber = "+254700000002",
                Gender = "male",
                ReceiveNotifications = true,
                Email = "brian.officer@apexassurance.co.ke",
                PasswordHash = defaultHash,
                BranchId = mainBranch.Id,
                RoleId = roles.GetValueOrDefault("ClaimsOfficer"),
                ApprovalLimit = 100000.00m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            // Underwriting Manager (Tier 2: < KSh 500,000)
            new()
            {
                FullName = "Faith Chebet",
                Username = "faith.chebet",
                PhoneNumber = "+254700000003",
                Gender = "female",
                ReceiveNotifications = true,
                Email = "faith.manager@apexassurance.co.ke",
                PasswordHash = defaultHash,
                BranchId = mainBranch.Id,
                RoleId = roles.GetValueOrDefault("UnderwritingManager"),
                ApprovalLimit = 500000.00m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            // Finance Director (Tier 3: > KSh 500,000)
            new()
            {
                FullName = "David Mutua",
                Username = "david.mutua",
                PhoneNumber = "+254700000004",
                Gender = "male",
                ReceiveNotifications = true,
                Email = "david.director@apexassurance.co.ke",
                PasswordHash = defaultHash,
                BranchId = mainBranch.Id,
                RoleId = roles.GetValueOrDefault("FinanceDirector"),
                ApprovalLimit = 5000000.00m, // KSh 5M limit for high-exposure settlements
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            // Compliance & Statutory Auditor (Read-only)
            new()
            {
                FullName = "Amina Hassan",
                Username = "amina.hassan",
                PhoneNumber = "+254700000005",
                Gender = "female",
                ReceiveNotifications = false,
                Email = "amina.auditor@apexassurance.co.ke",
                PasswordHash = defaultHash,
                BranchId = mainBranch.Id,
                RoleId = roles.GetValueOrDefault("ComplianceAuditor"),
                ApprovalLimit = 0.00m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }

        public static async Task SeedSubscriptionsAsync(AppDbContext context, ILicenseKeyService licenseKeyService)
        {
            if (await context.Subscriptions.AnyAsync()) return;

            var apexTenant = await context.Organizations.FirstOrDefaultAsync(o => o.Slug == "apex-general");
            if (apexTenant == null) return;

            var subscription = new Subscription
            {
                PlanRef = "TEST-SUB-01",
                PlanName = "Enterprise Plan",
                Status = "active",
                TopUpAiCreditsBalance = 0,
                OrganizationId = apexTenant.Id
            };

            await context.Subscriptions.AddAsync(subscription);
            await context.SaveChangesAsync();

            // 1. GENERATING A KEY
            string key = licenseKeyService.Generate(
                type: "demo",
                seats: 5,
                expires: true,
                expiresAt: DateTime.UtcNow.AddMonths(1)
            );

            var data = licenseKeyService.Decrypt(key);

            var pool = new LicensePool
            {
                LicenseType = data.Type ?? "demo",
                LicenseKey = key,
                Seats = data.Seats,
                FreeAiCreditsBalance = data.Seats * 20,
                Expires = data.Expires,
                ExpiresAt = data.ExpiresAt,
                SubscriptionId = subscription.Id
            };

            await context.LicensePools.AddAsync(pool);
            await context.SaveChangesAsync();

            // Assign user to the license pool
            var superAdmin = await context.Users.FirstOrDefaultAsync(u => u.Email == "superadmin@apexassurance.co.ke");
            if (superAdmin != null)
            {
                var userLicense = new UserLicense
                {
                    UserId = superAdmin.Id,
                    LicensePoolId = pool.Id,
                    AssignedAt = DateTime.UtcNow
                };

                await context.UserLicenses.AddAsync(userLicense);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedCustomersAndMembersAsync(AppDbContext context)
        {
            if (await context.Customers.AnyAsync()) return;

            var org = await context.Organizations.FirstOrDefaultAsync(o => o.Slug == "apex-general");
            if (org == null) return;

            string defaultHash = BCrypt.Net.BCrypt.HashPassword("ClaimsDesk@2026!");

            var aaron = new Customer
            {
                FullName = "Aaron Muuo",
                Email = "aaron.muuo@example.com",
                PhoneNumber = "+254700111222",
                NationalId = "12345678",
                PasswordHash = defaultHash,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var linda = new Customer
            {
                FullName = "Linda Musyoki",
                Email = "linda.musyoki@example.com",
                PhoneNumber = "+254700333444",
                NationalId = "87654321",
                PasswordHash = defaultHash,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await context.Customers.AddRangeAsync(aaron, linda);
            await context.SaveChangesAsync();

            var members = new List<Member>
            {
                new()
                {
                    OrganizationId = org.Id,
                    CustomerId = aaron.Id,
                    MemberNumber = "APX-MEM-2026-001",
                    Status = "Active",
                    JoinedDate = DateTime.UtcNow.AddYears(-1),
                    CreatedAt = DateTime.UtcNow
                },
                new()
                {
                    OrganizationId = org.Id,
                    CustomerId = linda.Id,
                    MemberNumber = "APX-MEM-2026-002",
                    Status = "Active",
                    JoinedDate = DateTime.UtcNow.AddMonths(-6),
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Members.AddRangeAsync(members);
            await context.SaveChangesAsync();
        }
    }
}
