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

            // Enforce foreign key relationship on Users
            modelBuilder.Entity<User>()
                .HasOne(u => u.Organization)
                .WithMany(o => o.Users)
                .HasForeignKey(u => u.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete of tenant data

            // Compound index: email must be unique per organization
            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.OrganizationId, u.Email })
                .IsUnique();
        }
    }
}
