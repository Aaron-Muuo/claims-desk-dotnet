using ClaimsDesk.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ClaimsDesk.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets represent the MySQL tables
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Organization> Organizations => Set<Organization>();

        public DbSet<Branch> Branches => Set<Branch>();
        public DbSet<AuditTrail> AuditTrails => Set<AuditTrail>();

        public DbSet<Subscription> Subscriptions => Set<Subscription>();
        public DbSet<LicensePool> LicensePools => Set<LicensePool>();
        public DbSet<UserLicense> UserLicenses => Set<UserLicense>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Member> Members => Set<Member>();
        public DbSet<Claim> Claims => Set<Claim>();
        public DbSet<ClaimWorkflowLog> ClaimWorkflowLogs => Set<ClaimWorkflowLog>();
        public DbSet<AiPromptTemplate> AiPromptTemplates => Set<AiPromptTemplate>();
        public DbSet<ClaimAiAnalysis> ClaimAiAnalyses => Set<ClaimAiAnalysis>();

        public DbSet<ApiKey> ApiKeys => Set<ApiKey>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure indexes and unique constraints

            modelBuilder.Entity<User>()
                .Property(u => u.ApprovalLimit)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Organization>()
            .HasIndex(o => o.Slug)
            .IsUnique();

            modelBuilder.Entity<Organization>()
                .HasIndex(o => o.LicenseNumber)
                .IsUnique();

            // Customer constraints: Unique email and national ID platform-wide
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.NationalId)
                .IsUnique();

            // Member constraints
            modelBuilder.Entity<Member>()
                .HasIndex(m => new { m.OrganizationId, m.MemberNumber })
                .IsUnique();

            modelBuilder.Entity<Member>()
                .HasIndex(m => new { m.OrganizationId, m.CustomerId })
                .IsUnique();

            modelBuilder.Entity<Member>()
                .HasOne(m => m.Organization)
                .WithMany(o => o.Members)
                .HasForeignKey(m => m.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Member>()
                .HasOne(m => m.Customer)
                .WithMany(c => c.Memberships)
                .HasForeignKey(m => m.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cascading delete for RolePermission
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany()
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany()
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Organization)
                .WithMany()
                .HasForeignKey(rp => rp.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RolePermission>()
                .HasIndex(rp => new { rp.RoleId, rp.PermissionId, rp.OrganizationId })
                .IsUnique();

            // Cascading delete for Organization -> Branches
            modelBuilder.Entity<Branch>()
                .HasOne(b => b.Organization)
                .WithMany(o => o.Branches)
                .HasForeignKey(b => b.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cascading delete for Organization -> Subscriptions
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Organization)
                .WithMany()
                .HasForeignKey(s => s.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cascading delete for Subscription -> LicensePools
            modelBuilder.Entity<LicensePool>()
                .HasOne(l => l.Subscription)
                .WithMany(s => s.LicensePools)
                .HasForeignKey(l => l.SubscriptionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cascading delete for LicensePool -> UserLicenses
            modelBuilder.Entity<UserLicense>()
                .HasOne(ul => ul.LicensePool)
                .WithMany(l => l.UserLicenses)
                .HasForeignKey(ul => ul.LicensePoolId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cascading delete for User -> UserLicenses
            modelBuilder.Entity<UserLicense>()
                .HasOne(ul => ul.User)
                .WithMany()
                .HasForeignKey(ul => ul.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cascading delete for Branch -> Users
            modelBuilder.Entity<User>()
                .HasOne(u => u.Branch)
                .WithMany(b => b.Users)
                .HasForeignKey(u => u.BranchId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cascading delete for Branch -> AuditTrails
            modelBuilder.Entity<AuditTrail>()
                .HasOne(a => a.Branch)
                .WithMany()
                .HasForeignKey(a => a.BranchId)
                .OnDelete(DeleteBehavior.Cascade);

            // Restrict delete for User -> AuditTrails (keep audit trails if user is deleted, or set null)
            modelBuilder.Entity<AuditTrail>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Compound index: email must be unique per branch
            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.BranchId, u.Email })
                .IsUnique();



            // Multi-tenant unique claim number per organization
            modelBuilder.Entity<Claim>()
                .HasIndex(c => new { c.OrganizationId, c.ClaimNumber })
                .IsUnique();

            // Prevent cascading deletes on multi-tenant foreign keys
            modelBuilder.Entity<Claim>()
                .HasOne(c => c.Organization)
                .WithMany()
                .HasForeignKey(c => c.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Claim>()
                .HasOne(c => c.Member)
                .WithMany()
                .HasForeignKey(c => c.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Claim>()
                .HasOne(c => c.CreatedByUser)
                .WithMany()
                .HasForeignKey(c => c.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Claim>()
                .HasOne(c => c.AssignedToUser)
                .WithMany()
                .HasForeignKey(c => c.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Claim>()
                .HasOne(c => c.ApprovedByUser)
                .WithMany()
                .HasForeignKey(c => c.ApprovedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClaimWorkflowLog>()
                .HasOne(l => l.Claim)
                .WithMany(c => c.WorkflowLogs)
                .HasForeignKey(l => l.ClaimId)
                .OnDelete(DeleteBehavior.Cascade);


            //AI claims

            modelBuilder.Entity<AiPromptTemplate>()
                .HasIndex(p => new { p.OrganizationId, p.StageKey, p.IsActive });

            modelBuilder.Entity<ClaimAiAnalysis>()
                .HasOne(a => a.Claim)
                .WithMany()
                .HasForeignKey(a => a.ClaimId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClaimAiAnalysis>()
                .HasOne(a => a.PromptTemplate)
                .WithMany()
                .HasForeignKey(a => a.PromptTemplateId)
                .OnDelete(DeleteBehavior.SetNull);

            //api keys
            modelBuilder.Entity<ApiKey>()
                .HasIndex(k => k.AppId)
                .IsUnique();

            // Index for filtering active keys by organization
            modelBuilder.Entity<ApiKey>()
                .HasIndex(k => new { k.OrganizationId, k.IsActive, k.Type });

            // Prevent cascading deletions when organization or user is removed
            modelBuilder.Entity<ApiKey>()
                .HasOne(k => k.Organization)
                .WithMany()
                .HasForeignKey(k => k.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApiKey>()
                .HasOne(k => k.CreatedByUser)
                .WithMany()
                .HasForeignKey(k => k.CreatedByUserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
